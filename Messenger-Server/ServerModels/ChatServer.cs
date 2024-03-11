using ModelLibrary.JsonModels;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ModelLibrary.ServerModels
{
    public class ChatServer
    {
         public Socket Socket { get; set; }
        public IPAddress Ip { get; set; }
        public IPEndPoint Ep { get; set; }
        public Action<Socket> worker = (s) =>
        {
            try
            {
                var buffer = new byte[10000000];
                var read = s.Receive(buffer);
                string raw = Encoding.UTF8.GetString(buffer, 0, read);
                Console.WriteLine(raw);
                DataMessage message = JsonSerializer.Deserialize<DataMessage>(raw);
                var response = "";
                switch (message.Type)
                {
                    case MessageType.RegisterMessage:
                        var register = DataCenter.CheckRegisterInfo(message);
                        response = JsonSerializer.Serialize(register);
                        break;
                    case MessageType.LoginMessage:
                        var login = DataCenter.CheckLoginInfo(message);
                        response = JsonSerializer.Serialize(login);
                        break;
                    case MessageType.ChatMessage:
                        var result = DataCenter.SendMessage(message);
                        response = JsonSerializer.Serialize(result);
                        break;
                    case MessageType.ContactsRequest:
                        var contacts = DataCenter.GetContacts(message.Data);
                        response = JsonSerializer.Serialize(contacts);
                        break;
                    case MessageType.FindContact:
                        var contact = DataCenter.FindContact(message.Data);
                        response = JsonSerializer.Serialize(contact);
                        break;
                    case MessageType.GetChat:
                        var chat = DataCenter.GetChat(message);
                        response = JsonSerializer.Serialize(chat);
                        break;
                    case MessageType.ChangeProfile:
                        var profile = DataCenter.ChangeProfile(message);
                        response = JsonSerializer.Serialize(profile);
                        break;
                    case MessageType.ChangePassword:
                        var res = DataCenter.ChangePassword(message);
                        response = JsonSerializer.Serialize(res);
                        break;
                    case MessageType.CodeRequest:
                        var code = DataCenter.GetEmailCode(message);
                        response = JsonSerializer.Serialize(code);
                        break;
                    case MessageType.ResetPassword:
                        var reset = DataCenter.ResetPassword(message);
                        response = JsonSerializer.Serialize(reset);
                        break;
                }
                var mes = Encoding.UTF8.GetBytes(response);
                Console.WriteLine(response);
                s.Send(mes);
                s.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        };
        public ChatServer()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            Ip = IPAddress.Parse("127.0.0.1");
            Ep = new IPEndPoint(Ip, 5000);
            Socket.Bind(Ep);
        }

        public void Run()
        {
            while (true)
            {

                Socket.Listen(1000);
                Socket ns = Socket.Accept();
                Console.WriteLine("New socket connected");
                Task.Run(() =>
                {
                    worker(ns);
                });
            }
        }
    }
}