using Microsoft.Win32;
using ModelLibrary.JsonModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Client_Wpf.Models
{
    public static class Helper
    {
        public static byte[]? ImageToBytes(BitmapImage? image)
        {
            if (image == null)
            {
                return null;
            }
            byte[] byteArray;
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(stream);
                byteArray = stream.ToArray();
            }
            return byteArray;
        }
        public static BitmapImage? ImageFromBytes(byte[]? bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            // Load the byte array into a memory stream
            MemoryStream stream = new MemoryStream(bytes);

            // Create a new BitmapImage object
            BitmapImage bitmapImage = new BitmapImage();

            // Set the BitmapImage object's source to the memory stream
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = stream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage;
        }
        public static BitmapImage OpenFromFile()
        {
            BitmapImage image = new();
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Image files (*.bmp, *.jpg, *.jpeg, *.png)|*.bmp;*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                image = new BitmapImage(new Uri(openFileDialog.FileName));
            }
            if (image.UriSource == null)
            {
                image = DefaultImage;
            }
            return image;
        }
        public static BitmapImage DefaultImage
        {
            get => new(new Uri("https://thumbs.dreamstime.com/b/default-avatar-profile-icon-vector-social-media-user-portrait-176256935.jpg"));

        }
        public static int CheckPasswordStrength(string password)
        {
            int score = 0;
            string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "0123456789";
            string specialSymbols = "!@#$%^&*()_+-=[]{}\\|;:\",./<>?";
            if (string.IsNullOrEmpty(password))
            {
                return score;
            }
            if (password.Length < 4)
            {
                return score;
            }

            if (password.Length >= 10 &&
                password.Any(x => numbers.Contains(x) &&
                password.Any(x => lowercaseLetters.Contains(x) &&
                password.Any(x => uppercaseLetters.Contains(x) &&
                password.Any(x => specialSymbols.Contains(x))))))
            {
                score = 5;
            }
            else if (password.Length >= 6 &&
                password.Any(x => numbers.Contains(x) &&
                password.Any(x => lowercaseLetters.Contains(x) &&
                password.Any(x => uppercaseLetters.Contains(x) &&
                password.Any(x => specialSymbols.Contains(x))))))
            {
                score = 4;
            }
            else if (password.Length >= 9 &&
                password.Any(x => numbers.Contains(x) &&
                password.Any(x => lowercaseLetters.Contains(x) &&
                password.Any(x => uppercaseLetters.Contains(x)))))
            {
                score = 4;
            }
            else if (password.Length >= 8 &&
               password.Any(x => numbers.Contains(x) &&
               password.Any(x => lowercaseLetters.Contains(x) &&
               password.Any(x => specialSymbols.Contains(x)))))
            {
                score = 4;
            }
            else if (password.Length >= 8 &&
               password.Any(x => uppercaseLetters.Contains(x) &&
               password.Any(x => lowercaseLetters.Contains(x) &&
               password.Any(x => specialSymbols.Contains(x)))))
            {
                score = 4;
            }
            else if (password.Length >= 5 &&
               password.Any(x => numbers.Contains(x) &&
               password.Any(x => lowercaseLetters.Contains(x) &&
               password.Any(x => uppercaseLetters.Contains(x)))))
            {
                score = 3;
            }
            else if (password.Length >= 5 &&
               password.Any(x => numbers.Contains(x) &&
               password.Any(x => lowercaseLetters.Contains(x) &&
               password.Any(x => specialSymbols.Contains(x)))))
            {
                score = 3;
            }
            else if (password.Length >= 5 &&
               password.Any(x => uppercaseLetters.Contains(x) &&
               password.Any(x => lowercaseLetters.Contains(x) &&
               password.Any(x => specialSymbols.Contains(x)))))
            {
                score = 3;
            }
            else if (password.Length >= 7 &&
            password.Any(x => uppercaseLetters.Contains(x) &&
            password.Any(x => specialSymbols.Contains(x))))
            {
                score = 3;
            }
            else if (password.Length >= 7 &&
           password.Any(x => numbers.Contains(x) &&
           password.Any(x => specialSymbols.Contains(x))))
            {
                score = 3;
            }
            else if (password.Length >= 7 &&
           password.Any(x => lowercaseLetters.Contains(x) &&
           password.Any(x => specialSymbols.Contains(x))))
            {
                score = 3;
            }
            else if (password.Length >= 5 &&
           password.Any(x => lowercaseLetters.Contains(x) &&
           password.Any(x => uppercaseLetters.Contains(x))))
            {
                score = 2;
            }
            else if (password.Length >= 5 &&
          password.Any(x => lowercaseLetters.Contains(x) &&
          password.Any(x => numbers.Contains(x))))
            {
                score = 2;
            }
            else if (password.Length >= 5 &&
          password.Any(x => uppercaseLetters.Contains(x) &&
          password.Any(x => numbers.Contains(x))))
            {
                score = 2;
            }
            else if (password.Length >= 5 &&
         password.Any(x => specialSymbols.Contains(x) &&
         password.Any(x => numbers.Contains(x))))
            {
                score = 2;
            }
            else if (password.All(x => specialSymbols.Contains(x)))
            {
                score = 2;
            }
            else if (password.Length <= 5)
            {
                score = 1;
            }
            else if (password.All(x => numbers.Contains(x)))
            {
                score = 1;
            }
            else if (password.All(x => lowercaseLetters.Contains(x)))
            {
                score = 1;
            }
            else if (password.All(x => uppercaseLetters.Contains(x)))
            {
                score = 1;
            }
            return score;
        }
        public static DataMessage SendToServer(DataMessage message)
        {
                var Ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                socket.Connect(Ep);
                var str = JsonSerializer.Serialize(message);
                var bytes = Encoding.UTF8.GetBytes(str);
                socket.Send(bytes);
                var response = new byte[10000000];
                var read = socket.Receive(response);
                var responsestr = Encoding.UTF8.GetString(response, 0, read);
                socket.Close();
                return JsonSerializer.Deserialize<DataMessage>(responsestr);
        }
        public static BitmapImage CheckImage(string filename, string imageurl)
        {
            if (!File.Exists(filename))
            {
                Directory.CreateDirectory("Images");
                var image = new BitmapImage(new Uri(imageurl));
                // Create a new bitmap encoder and set it to encode a PNG file
                BitmapEncoder encoder = CheckImageExtension(image);

                // Create a new memory stream to write the bitmap data to
                MemoryStream memoryStream = new MemoryStream();

                // Add the bitmap image to the encoder
                encoder.Frames.Add(BitmapFrame.Create(image));

                // Save the bitmap to the memory stream
                encoder.Save(memoryStream);

                // Write the memory stream to the file
                File.WriteAllBytes(filename, memoryStream.ToArray());

            }
            BitmapImage bitmapImage = new BitmapImage();

            // Set the bitmap image's UriSource to the file path
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(filename, UriKind.RelativeOrAbsolute);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage;
        }
        public static BitmapEncoder CheckImageExtension(BitmapImage image)
        {
            switch (Path.GetExtension(image.UriSource.ToString()))
            {
                case "jpg":
                    return new JpegBitmapEncoder();
                case "jpeg":
                    return new JpegBitmapEncoder();
                case "png":
                    return new PngBitmapEncoder();
                case "bmp":
                    return new BmpBitmapEncoder();
                default:
                    return new PngBitmapEncoder();
            }
        }
    }
}
