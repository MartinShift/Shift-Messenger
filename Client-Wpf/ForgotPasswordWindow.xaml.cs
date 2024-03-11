using Client_Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client_Wpf
{
    /// <summary>
    /// Interaction logic for ForgotPasswordWindow.xaml
    /// </summary>
    public partial class ForgotPasswordWindow : Window
    {
        public ForgotPasswordWindow(string login)
        {
            InitializeComponent();
            DataContext = new ForgotPasswordWindowViewModel(login)
            {
                Window = this
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
                ((dynamic)DataContext).NewPassword = Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
        private void PasswordBox_ConfirmPasswordChanged(object sender, RoutedEventArgs e)
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
                ((dynamic)DataContext).ConfirmPassword = Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
