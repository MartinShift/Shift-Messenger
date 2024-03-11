using Client_Wpf.Models;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ModelLibrary.JsonModels;
using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client_Wpf.ViewModels
{
    public class LoginWindowViewModel : NotifyPropertyChangedBase
    {
        public LoginWindowViewModel()
        {
            SavedClients = new();
            LoadClients();
        }
        private List<Tutel> SavedClients { get; set; }
        private string _login { get; set; }
        public string Login
        {
            get => _login; set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
                var client = SavedClients.FirstOrDefault(x => x.login == _login);
                if (client != null)
                {
                    window.PasswordChanged(client.password);
                }
            }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));      
            }
        }
        private bool _savePassword;
        public bool SavePassword { get => _savePassword; set { _savePassword = value; OnPropertyChanged(nameof(SavePassword)); } }
        public LoginWindow window { get; set; }
        public Socket Socket { get; set; }
        public JsonClient Client { get; set; }
        public ICommand TryLogin => new RelayCommand(x =>
        {
            var message = new LoginMessage()
            {
                Login = Login,
                Password = Password
            };
            var datamessage = new DataMessage()
            {
                Data = JsonSerializer.Serialize(message),
                Type = MessageType.LoginMessage
            };
            var json = Helper.SendToServer(datamessage);

            switch (json.Type)
            {
                case MessageType.LoginSuccess:
                    Client = JsonSerializer.Deserialize<JsonClient>(json.Data);
                    if (SavePassword == true)
                    {
                        if (!Directory.Exists($"SavedClients"))
                        {
                            Directory.CreateDirectory("SavedClients");
                        }
                        if (!File.Exists($"SavedClients\\{Login}.json"))
                        {
                            File.WriteAllText($"SavedClients\\{Login}.json", JsonSerializer.Serialize(new Tutel() { login = Login, password = Password }));
                        }
                    }
                    MessageBox.Show("Success!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    window.Close();
                    break;
                case MessageType.LoginFailure:
                    var error = JsonSerializer.Deserialize<LoginResults>(json.Data);
                    switch (error)
                    {
                        case LoginResults.IncorrectPassword:
                            MessageBox.Show("Incorrect Password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case LoginResults.NoLogin:
                            MessageBox.Show("Incorrect Login!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                    break;
            }
        });
        public ICommand ForgotPassword => new RelayCommand(x =>
        {
            var window = new EnterEmailCode();
            window.ShowDialog();
        });
        public ICommand Register => new RelayCommand(x =>
        {
            var registerwindow = new RegisterWindow();
            registerwindow.ShowDialog();

            var context = registerwindow.DataContext as RegisterWindowViewModel;
            var client = context.Client;
            if (client != null)
            {
                Client = client;
            }
            this.window.Close();
        });
        public void LoadClients()
        {
            if (!Directory.Exists($"SavedClients"))
            {
                Directory.CreateDirectory("SavedClients");
            }
            Directory.GetFiles("SavedClients").ToList().ForEach(x => SavedClients.Add(JsonSerializer.Deserialize<Tutel>(File.ReadAllText(x))));
        }
    }
}
