using ModelLibrary.DbModels;
using ModelLibrary.JsonModels;
using System.Net.Mail;
using System.Net;
using System.Text.Json;
using BCrypt.Net;

namespace ModelLibrary.ServerModels
{
    public static class DataCenter
    {
        public static DataMessage CheckRegisterInfo(DataMessage raw)
        {
            RegisterMessage message = JsonSerializer.Deserialize<RegisterMessage>(raw.Data);
            var context = new MessengerDbContext();
            var client = context.Clients.FirstOrDefault(x => x.Login == message.Login);
            if (client != null)
            {
                return new DataMessage()
                {
                    Data = JsonSerializer.Serialize(RegisterResults.LoginExists),
                    Type = MessageType.RegisterFailure
                };
            }
            else
            {
                var jsonclient = CreateClient(message);
                context.Clients.Add(new Client()
                {
                    Login = message.Login,
                    Password = BCrypt.Net.BCrypt.HashPassword(message.Password),
                    Logo = message.Logo,
                    NickName = message.NickName,
                    Email = message.Email
                });
                context.SaveChanges();
                return new DataMessage()
                {
                    Data = JsonSerializer.Serialize(jsonclient),
                    Type = MessageType.RegisterSuccess
                };
            }
        }
        public static DataMessage CheckLoginInfo(DataMessage raw)
        {
            LoginMessage message = JsonSerializer.Deserialize<LoginMessage>(raw.Data);
            var context = new MessengerDbContext();
            var client = context.Clients.FirstOrDefault(x => x.Login == message.Login);
            if (client == null)
            {
                return new DataMessage()
                {
                    Data = JsonSerializer.Serialize(LoginResults.NoLogin),
                    Type = MessageType.LoginFailure
                };
            }     
            else if (!BCrypt.Net.BCrypt.Verify(message.Password, client.Password))
            {
                return new DataMessage()
                {
                    Data = JsonSerializer.Serialize(LoginResults.IncorrectPassword),
                    Type = MessageType.LoginFailure
                };
            }
            else
            {
                var json = CreateClient(client);
                return new DataMessage() { Data = JsonSerializer.Serialize(json), Type = MessageType.LoginSuccess };
            }
        }
        public static DataMessage GetContacts(string login)
        {
            var context = new MessengerDbContext();
            var client = context.Clients.FirstOrDefault(x => x.Login == login);
            if (client == null)
            {
                var clients = new List<JsonClient>();
                return new DataMessage()
                {
                    Data = JsonSerializer.Serialize(clients),
                    Type = MessageType.ContactsRequest
                };
            }
            else
            {
                var clients = context.Clients.Where(x => x.SentMessages.Any(x => x.ToClient.Login == login) || x.ReceivedMessages.Any(x => x.FromClient.Login == login)).Select(x => new JsonClient() { Login = x.Login, Logo = x.Logo, NickName = x.NickName }).ToList();

                return new DataMessage()
                {
                    Data = JsonSerializer.Serialize(clients),
                    Type = MessageType.ContactsRequest
                };

            };
        }
        public static DataMessage FindContact(string login)
        {
            var context = new MessengerDbContext();
            var client = context.Clients.FirstOrDefault(x => login == x.Login);
            if (client == null)
            {
                var message = new DataMessage()
                {
                    Data = "",
                    Type = MessageType.ContactNotFound
                };
                return message;
            }
            else
            {
                var json = new JsonClient()
                {
                    Login = client.Login,
                    Logo = client.Logo,
                    NickName = client.NickName,
                };
                var message = new DataMessage()
                {
                    Data = JsonSerializer.Serialize(json),
                    Type = MessageType.ContactFound
                };
                return message;
            }
        }
        public static DataMessage GetChat(DataMessage raw)
        {
            MessageFind find = JsonSerializer.Deserialize<MessageFind>(raw.Data);
            var context = new MessengerDbContext();
            var fromclient = context.Clients.First(x => x.Login == find.FromLogin);
            var toclient = context.Clients.First(x => x.Login == find.ToLogin);
            var chat = context.Messages.Where(x => x.FromClient.Login == fromclient.Login && x.ToClient.Login == toclient.Login || x.ToClient.Login == fromclient.Login && x.FromClient.Login == toclient.Login)
                .Select(x => new ChatMessage()
                {
                    Date = x.Time,
                    From = CreateClient(x.FromClient),
                    To = CreateClient(x.ToClient),
                    Image = x.Image,
                    String = x.String
                }).ToList();
            var message = new DataMessage()
            {
                Data = JsonSerializer.Serialize<List<ChatMessage>>(chat),
                Type = MessageType.GetChat
            };
            return message;
        }
        public static DataMessage SendMessage(DataMessage raw)
        {
            var message = JsonSerializer.Deserialize<ChatMessage>(raw.Data);
            var context = new MessengerDbContext();
            var fromclient = context.Clients.First(x => x.Login == message.From.Login);
            var toclient = context.Clients.First(x => x.Login == message.To.Login);
            var dbmessage = new DbMessage()
            {
                FromClient = fromclient,
                ToClient = toclient,
                String = message.String,
                Time = message.Date,
                Image = message.Image,
            };
            context.Messages.Add(dbmessage);
            context.SaveChanges();

            var data = new DataMessage()
            {
                Data = "",
                Type = MessageType.ChatMessage
            };
            return data;
        }
        public static DataMessage ChangeProfile(DataMessage message)
        {
            var Client = JsonSerializer.Deserialize<JsonClient>(message.Data);
            var context = new MessengerDbContext();
            var client = context.Clients.First(x => x.Login == Client.Login);
            client.NickName = Client.NickName;
            client.Logo = Client.Logo;
            context.SaveChanges();
            try
            {
                var smtpclient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("mori.steamer@gmail.com", "ldmthoshlrgrqedk"),
                    EnableSsl = true,

                };
                var mail = new MailMessage()
                {
                    From = new MailAddress("mori.steamer@gmail.com", "Shift Support"),
                    Subject = "Email test",
                    Body = $"This is a test if email is correct"
                };
                mail.To.Add($"{Client.Email}");
                smtpclient.Send(mail);
            }
            catch 
            {
                return new DataMessage()
                {
                    Data = "Incorrect Email",
                    Type = MessageType.IncorrectEmailChange
                };
            }
            client.Email = Client.Email;
            context.SaveChanges();
            return new DataMessage()
            {
                Data = "",
                Type = MessageType.ChangeProfile
            };
        }
        public static DataMessage ChangePassword(DataMessage message)
        {
            var data = JsonSerializer.Deserialize<PasswordChangeMessage>(message.Data);
            var context = new MessengerDbContext();
            var client = context.Clients.First(x => x.Login == data.ClientLogin);
            if (data.OldPassword != client.Password)
            {
                return new DataMessage()
                {
                    Data = "Incorrect Password!",
                    Type = MessageType.PasswordChangeFailure
                };
            }
            else
            {
                client.Password = data.NewPassword;
                context.SaveChanges();
                return new DataMessage()
                {
                    Data = "Success!",
                    Type = MessageType.PasswordChangeSuccess
                };
            }
        }
        public static DataMessage GetEmailCode(DataMessage message)
        {
            var Login = message.Data;
            var context = new MessengerDbContext();
            var client = context.Clients.First(x => x.Login == Login);
            CodeResults result;
            if (client == null)
            {
                result = CodeResults.WrongLogin;
                return new DataMessage()
                {
                    Data = JsonSerializer.Serialize(result),
                    Type = MessageType.CodeRequestFailure
                };
            }
            else
            {
                try
                {
                    var smtpclient = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential("mori.steamer@gmail.com", "ldmthoshlrgrqedk"),
                        EnableSsl = true,

                    };
                    var code = new Random().Next(100000, 999999);
                    var mail = new MailMessage()
                    {
                        From = new MailAddress("mori.steamer@gmail.com", "Shift Support"),
                        Subject = "Email verification",
                        Body = $"Your verification code is: {code}"
                    };
                    mail.To.Add($"{client.Email}");
                    smtpclient.Send(mail);
                    return new DataMessage()
                    {
                        Data = code.ToString(),
                        Type = MessageType.CodeRequestSuccess
                    };
                }
                catch (Exception ex)
                {
                    result = CodeResults.LoginEmailIsWrong;
                    return new DataMessage()
                    {
                        Data = JsonSerializer.Serialize(result),
                        Type = MessageType.CodeRequestFailure
                    };
                }
            }
        }
        public static DataMessage ResetPassword(DataMessage message)
        {
            var reset = JsonSerializer.Deserialize<ResetPasswordMessage>(message.Data);
            var context = new MessengerDbContext();
            var client = context.Clients.First(x => x.Login == reset.Login);
            client.Password = BCrypt.Net.BCrypt.HashPassword(reset.Password);
            context.SaveChanges();
            return new DataMessage()
            {
                Data = "",
                Type = MessageType.ResetPassword
            };
        }
        public static JsonClient CreateClient(RegisterMessage message)
        {
            var client = new JsonClient()
            {
                Login = message.Login,
                NickName = message.NickName,
                Logo = message.Logo,
                Email = message.Email
            };
            return client;
        }
        public static JsonClient CreateClient(Client message)
        {
            var client = new JsonClient()
            {
                Login = message.Login,
                NickName = message.NickName,
                Logo = message.Logo,
                Email = message.Email
            };
            return client;
        }
    }
}
