using Client_Wpf.Models;
using ModelLibrary.JsonModels;
using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client_Wpf.ViewModels
{
    public class EnterEmailCodeViewModel : NotifyPropertyChangedBase
    {
        private string _Login { get; set; }
        public EnterEmailCode Window { get; set; }
        public string Login { get => _Login; set { _Login = value; OnPropertyChanged(nameof(Login)); } }
        private int _Code { get; set; } = 0;
        private int _EnterCode { get; set; } = 0;
        public int EnterCode { get => _EnterCode; set { _EnterCode = value; OnPropertyChanged(nameof(EnterCode)); } }
        public ICommand GetCode => new RelayCommand(x =>
        {
            var message = new DataMessage()
            {
                Data = Login,
                Type = MessageType.CodeRequest
            };
            var response = Helper.SendToServer(message);
            switch (response.Type)
            {
                case MessageType.CodeRequestFailure:
                    switch (JsonSerializer.Deserialize<CodeResults>(response.Data))
                    {
                        case CodeResults.WrongLogin:
                            MessageBox.Show("Wrong login!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case CodeResults.LoginEmailIsWrong:
                            MessageBox.Show("This login Has Wrong or no Email!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;

                    }
                    break;
                case MessageType.CodeRequestSuccess:
                    _Code = int.Parse(response.Data);
                    break;
            }
        });
        public ICommand Submit => new RelayCommand(x =>
        {
            if(EnterCode != _Code)
            {
                MessageBox.Show("Incorrect code!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var window = new ForgotPasswordWindow(Login);
                window.ShowDialog();

                Window.Close();
            }
        });
    }
}
