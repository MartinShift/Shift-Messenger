using Client_Wpf.ViewModels;
using ModelLibrary.JsonModels;
using System.Runtime.InteropServices;
using System.Security;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Client_Wpf
{
    /// <summary>
    /// Interaction logic for PasswordChangeWindow.xaml
    /// </summary>
    public partial class PasswordChangeWindow : Window
    {
        public PasswordChangeWindow(JsonClient client)
        {
            DataContext = new PasswordChangeWindowViewModel(client)
            {
                Window = this
            };
            InitializeComponent();
        }
        private void PasswordBox_OldPasswordChanged(object sender, RoutedEventArgs e)
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
                ((dynamic)DataContext).OldPassword = Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
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
