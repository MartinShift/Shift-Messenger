using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModelLibrary.JsonModels
{
    public class RegisterMessage
    {
        public string NickName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public byte[] Logo { get; set; }
        public string? Email { get; set; }
    }
}
