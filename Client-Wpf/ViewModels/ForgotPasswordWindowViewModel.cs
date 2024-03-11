using Client_Wpf.Models;
using ModelLibrary.JsonModels;
using My.BaseViewModels;
using System.Text.Json;
using System.Windows.Input;
using System.Windows.Media;

namespace Client_Wpf.ViewModels
{
    public class ForgotPasswordWindowViewModel : NotifyPropertyChangedBase
    {
        public ForgotPasswordWindowViewModel(string login)
        {
            this.Login = login;
        }
        public string Login { get; set; }
        public ForgotPasswordWindow Window { get; set; }
        private string _NewPassword { get; set; } = string.Empty;
        public string NewPassword { get => _NewPassword; set { _NewPassword = value; OnPropertyChanged(nameof(NewPassword)); OnPropertyChanged(nameof(PasswordStrength)); OnPropertyChanged(nameof(Error)); OnPropertyChanged(nameof(ErrorColor)); } }
        private string _ConfirmPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get => _ConfirmPassword; set { _ConfirmPassword = value; OnPropertyChanged(nameof(ConfirmPassword)); OnPropertyChanged(nameof(PasswordStrength)); OnPropertyChanged(nameof(Error)); OnPropertyChanged(nameof(ErrorColor)); } }
        public string Error
        {
            get
            {
                if (NewPassword.Length < 4)
                {
                    return "Password must be 4 or more characters!";
                }
                if (NewPassword != ConfirmPassword)
                {
                    return "Passwords don't match!";
                }
                return "";
            }
        }
        public int PasswordStrength { get => Helper.CheckPasswordStrength(NewPassword) * 20; }
        public Brush ErrorColor { get; set; }
        public ICommand Submit => new RelayCommand(x =>
        {
            var data = new ResetPasswordMessage()
            {
                Login = Login,
                Password = NewPassword
            };
            var message = new DataMessage()
            {
                Data = JsonSerializer.Serialize(data),
                Type = MessageType.ResetPassword
            };
            var response = Helper.SendToServer(message);
            Window.Close();
        }, x => NewPassword == ConfirmPassword && NewPassword.Length >=4);
    }
}
