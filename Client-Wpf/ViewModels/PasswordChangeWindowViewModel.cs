using Client_Wpf.Models;
using ModelLibrary.JsonModels;
using My.BaseViewModels;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Client_Wpf.ViewModels
{
    public class PasswordChangeWindowViewModel : NotifyPropertyChangedBase
    {
        public PasswordChangeWindowViewModel(JsonClient client)
        {
            Client = client;
        }
        public PasswordChangeWindow Window { get; set; }
        public JsonClient Client { get; set; }
        private string _OldPassword { get; set; } = string.Empty;
        public string OldPassword { get => _OldPassword; set { _OldPassword = value; OnPropertyChanged(nameof(OldPassword)); OnPropertyChanged(nameof(Error)); OnPropertyChanged(nameof(ErrorColor)); } }
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
        public ICommand TrySubmit => new RelayCommand(x =>
        {
            var message = new PasswordChangeMessage()
            {
                OldPassword = OldPassword,
                NewPassword = NewPassword,
                ClientLogin = Client.Login
            };
            var Data = new DataMessage()
            {
                Data = JsonSerializer.Serialize(message),
                Type = MessageType.ChangePassword
            };
            var response = Helper.SendToServer(Data);
            switch(response.Type)
            {
                case MessageType.PasswordChangeFailure:
                    MessageBox.Show(response.Data,"Error",MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case MessageType.PasswordChangeSuccess:
                    MessageBox.Show(response.Data, "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                    Window.Close();
                    break;
            }
        }, x => NewPassword == ConfirmPassword && NewPassword.Length >= 4);
    }
}
