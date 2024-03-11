using Client_Wpf.Models;
using ModelLibrary.JsonModels;
using My.BaseViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Client_Wpf.ViewModels;

public class ClientViewModel : NotifyPropertyChangedBase
{
    public JsonClient Client { get; set; }
    public ClientViewModel(JsonClient client)
    {
        Client = client;
    }
    public string Nickname { get => Client.NickName; set { Client.NickName = value; OnPropertyChanged(nameof(Nickname)); } }
    public string Login { get => Client.Login; set { Client.Login = value; OnPropertyChanged(nameof(Login)); } }
    public BitmapImage Logo
    {
        get
        {
            return Helper.ImageFromBytes(Client.Logo);
        }
        set
        {
            Client.Logo = Helper.ImageToBytes(value);
        }
    }
    public string? Email { get => Client.Email; set { Client.Email = value; OnPropertyChanged(nameof(Email)); } }

}
