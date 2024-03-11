using Client_Wpf.Models;
using Microsoft.Win32;
using ModelLibrary.JsonModels;
using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Client_Wpf.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        public MainWindowViewModel()
        {
            Contacts = new();
            Chat = new();
            SendButtonImage = new BitmapImage(new Uri("https://www.pngkit.com/png/detail/188-1882365_send-button-png-send-button-icon-png.png"));
            SelectImageImage = new BitmapImage(new Uri("https://cdn.iconscout.com/icon/free/png-256/free-photo-size-select-actual-1782180-1512958.png"));
            OnPropertyChanged(nameof(SendButtonImage));
            Task.Run(async () =>
            {
                MessageLoader();
            });
        }
        //Server
        public ClientViewModel? Client { get; set; }
        //Chat
        private string _Info { get; set; }
        public string Info { get => _Info; set { _Info = value; OnPropertyChanged(nameof(Info)); } }
        public BitmapImage Image { get; set; }
        private ClientViewModel? _Receiver { get; set; }
        public ClientViewModel? Receiver
        {
            get => _Receiver; set
            {
                _Receiver = value; OnPropertyChanged(nameof(Receiver));
            }
        }
        public List<ClientViewModel> Contacts { get; set; }
        public ObservableCollection<ClientViewModel> ContactsView { get => new(Contacts); }
        public List<MessageViewModel> Chat { get; set; }
        public ObservableCollection<MessageViewModel> ChatView { get => new(Chat); }
        public BitmapImage SendButtonImage { get; set; }
        public BitmapImage SelectImageImage { get; set; }
        public Visibility ProfileVisibility { get => Client == null ? Visibility.Hidden : Visibility.Visible; }
        public Visibility ProfileNullVisibility { get => Client == null ? Visibility.Visible : Visibility.Hidden; }
        //File 

        //Search
        private string _SearchLogin { get; set; } = string.Empty;
        public string SearchLogin { get => _SearchLogin; set { _SearchLogin = value; OnPropertyChanged(nameof(SearchLogin)); } }
        public ICommand Register => new RelayCommand(x =>
        {
            var window = new RegisterWindow();
            window.ShowDialog();

            var context = window.DataContext as RegisterWindowViewModel;
            var client = context.Client;
            if (client != null)
            {
                Client = new(client);
                OnPropertyChanged(nameof(Client));
            }
        });
        public ICommand Logout => new RelayCommand(x =>
        {
            Chat.Clear();
            OnPropertyChanged(nameof(ChatView));
            Contacts.Clear();
            OnPropertyChanged(nameof(ContactsView));
            Receiver = null;
            Client = null;
            OnPropertyChanged(nameof(Client));
            OnPropertyChanged(nameof(ProfileVisibility));
            OnPropertyChanged(nameof(ProfileNullVisibility));
        });
        public ICommand Login => new RelayCommand(x =>
        {
            var window = new LoginWindow();
            window.ShowDialog();

            var context = window.DataContext as LoginWindowViewModel;
            var client = context.Client;
            if (client != null)
            {
                Client = new(client);
                OnPropertyChanged(nameof(Client));
                OnPropertyChanged(nameof(ProfileVisibility));
                OnPropertyChanged(nameof(ProfileNullVisibility));
                LoadContacts();
            }

        });
        public ICommand FindContact => new RelayCommand(x =>
        {
            var message = new DataMessage()
            {
                Data = SearchLogin,
                Type = MessageType.FindContact
            };
            DataMessage response;
            response = Helper.SendToServer(message);

            switch (response.Type)
            {
                case MessageType.ContactNotFound:
                    MessageBox.Show("Contact not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case MessageType.ContactFound:
                    var contact = JsonSerializer.Deserialize<JsonClient>(response.Data);
                    Contacts.Add(new(contact));
                    Image = null;
                    Receiver = Contacts.Last();
                    Info = $"{Client.Nickname} Just added you!";
                    SendMessage.Execute(null);
                    LoadContacts();
                    break;
            }
        }, x => Client == null ? SearchLogin != "" : SearchLogin != Client.Login);
        public ICommand SendMessage => new RelayCommand(x =>
        {
            byte[] image = null;
            if (Image != null)
            {
                byte[] imageData = Helper.ImageToBytes(Image);
                image = imageData;
            }

            var message = new ChatMessage()
            {
                Date = DateTime.Now,
                From = Client.Client,
                To = Receiver.Client,
                Image = image,
                String = Info
            };

            string json = JsonSerializer.Serialize(message);
            var mes = new DataMessage()
            {
                Data = json,
                Type = MessageType.ChatMessage
            };
            DataMessage response;
            response = Helper.SendToServer(mes);
            Info = "";
            Image = null;
            OnPropertyChanged(nameof(Image));
        });
        public ICommand LoadImage => new RelayCommand(x =>
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Image files (*.bmp, *.jpg, *.jpeg, *.png)|*.bmp;*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                Image = new BitmapImage(new Uri(openFileDialog.FileName));
                OnPropertyChanged(nameof(Image));
            }
        });
        public ICommand ChangeImage => new RelayCommand(x =>
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Image files (*.bmp, *.jpg, *.jpeg, *.png)|*.bmp;*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                var image = new BitmapImage(new Uri(openFileDialog.FileName));
                if (image != null)
                {
                    Client.Logo = image;
                    OnPropertyChanged(nameof(Client));

                }
            }
        });
        public ICommand ChangeProfile => new RelayCommand(x =>
        {
            var message = new DataMessage()
            {
                Data = JsonSerializer.Serialize(Client.Client),
                Type = MessageType.ChangeProfile
            };
            var response = Helper.SendToServer(message);
            switch (response.Type)
            {
                case MessageType.IncorrectEmailChange:
                    MessageBox.Show("Wrong Email!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case MessageType.ChangeProfile:
                    MessageBox.Show("Profile Changed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }

        }, x => Client != null);
        public ICommand ChangePassword => new RelayCommand(x =>
        {
            var window = new PasswordChangeWindow(Client.Client);
            window.ShowDialog();
        });
        private async void LoadContacts()
        {

            var message = new DataMessage()
            {
                Data = Client.Login,
                Type = MessageType.ContactsRequest
            };
            DataMessage response = Helper.SendToServer(message);
            var contacts = JsonSerializer.Deserialize<List<JsonClient>>(response.Data);
            Contacts = contacts.Select(x => new ClientViewModel(x)).ToList();
            OnPropertyChanged(nameof(ContactsView));
        }
        private async void MessageLoader()
        {
            while (true)
            {
                if (Client != null && Receiver != null)
                {
                    LoadMessages();
                }
                Thread.Sleep(5000);
            }
        }
        private async void LoadMessages()
        {
            var message = new MessageFind()
            {
                FromLogin = Client.Login,
                ToLogin = Receiver.Login,
            };
            var datamessage = new DataMessage()
            {
                Data = JsonSerializer.Serialize(message),
                Type = MessageType.GetChat
            };
            DataMessage response = Helper.SendToServer(datamessage);
            var json = JsonSerializer.Deserialize<List<ChatMessage>>(response.Data);
            Chat = json.Select(x => new MessageViewModel(x)).ToList();
            Chat = Chat.OrderByDescending(x => x.Date).ToList();
            OnPropertyChanged(nameof(ChatView));
        }

    }
}
