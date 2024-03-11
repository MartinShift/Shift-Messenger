using Client_Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel).Login.Execute(null);
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


    }
}
