using Client_Wpf.ViewModels;
using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Client_Wpf
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginWindowViewModel()
            {
                window = this,
            };
        }
       private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var securePassword = new SecureString();
            if (DataContext != null)
            { securePassword = ((PasswordBox)sender).SecurePassword; }
            if (securePassword == null)
            {
                return;
            }
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                ((dynamic)DataContext).Password = Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
        private Storyboard shimmerStoryboard;
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            StartShimmerAnimation(button);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            StopShimmerAnimation(button);
        }

        private void StartShimmerAnimation(Button button)
        {
            shimmerStoryboard = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 1,
                To = 0.5,
                Duration = TimeSpan.FromSeconds(0.5),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            Storyboard.SetTarget(animation, button);
            Storyboard.SetTargetProperty(animation, new PropertyPath(OpacityProperty));

            shimmerStoryboard.Children.Add(animation);
            button.BeginStoryboard(shimmerStoryboard);
        }

        private void StopShimmerAnimation(Button button)
        {
            if (shimmerStoryboard != null)
            {
                button.BeginStoryboard(shimmerStoryboard);
                shimmerStoryboard.Stop();
                button.Resources.Remove("ShimmerAnimation");
                shimmerStoryboard = null;
            }
        }

        public void PasswordChanged(string value)
        {
            Passwordbox.Password = value;
        }

    }
}
