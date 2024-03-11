using Client_Wpf.Models;
using ModelLibrary.JsonModels;
using My.BaseViewModels;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Client_Wpf.ViewModels
{


    public class RegisterWindowViewModel : NotifyPropertyChangedBase
    {

        //server
        public Socket Socket { get; set; }
        public JsonClient? Client { get; set; }
        //input
        private string _NickName { get; set; } = string.Empty;
        public string NickName
        {
            get => _NickName; set
            {
                _NickName = value; OnPropertyChanged(nameof(NickName)); OnPropertyChanged(nameof(Error));
                OnPropertyChanged(nameof(Error));
            }
        }
        private string _Login { get; set; } = string.Empty;
        public string Login
        {
            get => _Login; set
            {
                _Login = value;
                OnPropertyChanged(nameof(Login));
                OnPropertyChanged(nameof(Error));
            }
        }
        private string _Email { get; set; } = string.Empty;
        public string Email
        {
            get => _Email; set
            {
                _Email = value;
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(Error));
            }
        }
        private string _Password { get; set; } = string.Empty;
        public string Password
        {
            get => _Password; set
            {
                _Password = value;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(Error));
                OnPropertyChanged(nameof(PasswordStrength));
            }
        }
        private string _ConfirmPassword { get; set; } = string.Empty;

        public string ConfirmPassword
        {
            get => _ConfirmPassword; set
            {
                _ConfirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                OnPropertyChanged(nameof(Error));
                OnPropertyChanged(nameof(PasswordStrength));
            }
        }
        public string Error
        {
            get
            {
                if (NickName.Length < 3)
                {
                    return "Nickname must be 3 or more characters!";
                }
                if (Password.Length < 4)
                {
                    return "Password must be 4 or more characters!";
                }
                if (Login.Length < 3)
                {
                    return "Login must be 3 or more characters!";
                }
                if (Password != ConfirmPassword)
                {
                    return "Passwords don't match!";
                }
                return "";
            }
        }
        public int PasswordStrength { get => Helper.CheckPasswordStrength(Password) * 20; }
        public Brush ErrorColor { get; set; }

        public BitmapImage Logo { get; set; }
        //data
        public RegisterWindow window { get; set; }
        public RegisterWindowViewModel()
        {

            Logo = new BitmapImage(new Uri("https://thumbs.dreamstime.com/b/default-avatar-profile-icon-vector-social-media-user-portrait-176256935.jpg"));
            OnPropertyChanged(nameof(Logo));
        }
        //Commands
        public ICommand TrySubmit => new RelayCommand(x =>
        {
            var message = new RegisterMessage()
            {
                Login = Login,
                Password = Password,
                Logo = Helper.ImageToBytes(Logo),
                NickName = NickName,
                Email = Email
            };
            var datamessage = new DataMessage()
            {
                Data = JsonSerializer.Serialize(message),
                Type = MessageType.RegisterMessage
            };
            var json = Helper.SendToServer(datamessage);
            switch (json.Type)
            {
                case MessageType.RegisterSuccess:
                    Client = JsonSerializer.Deserialize<JsonClient>(json.Data);
                    MessageBox.Show("Success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    window.Close();
                    break;
                case MessageType.RegisterFailure:
                    MessageBox.Show("This login already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Login = "";
                    break;
            }
        }, x => Password.Length >= 4 && NickName.Length >= 3 && Login.Length >= 3 && Password == ConfirmPassword);
        public ICommand LoadLogo => new RelayCommand(x =>
        {
            Logo = Helper.OpenFromFile();
            OnPropertyChanged(nameof(Logo));
        });

    }
}
