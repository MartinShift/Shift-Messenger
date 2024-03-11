using Client_Wpf.Models;
using ModelLibrary.JsonModels;
using My.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Client_Wpf.ViewModels
{
    public class MessageViewModel : NotifyPropertyChangedBase
    {
        public ChatMessage Message { get; set; }
        public MessageViewModel(ChatMessage message) { Message = message; }
        public string? String { get => Message.String; set { Message.String = value; OnPropertyChanged(nameof(String)); } }
        public BitmapImage? image { get => Helper.ImageFromBytes(Message.Image); set { Message.Image = Helper.ImageToBytes(value); OnPropertyChanged(nameof(Image)); } }
        public DateTime Date { get => Message.Date; set { Message.Date = value; OnPropertyChanged(nameof(Date)); } }

    }
}
