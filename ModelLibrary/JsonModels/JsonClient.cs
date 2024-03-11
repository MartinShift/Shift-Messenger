using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.JsonModels
{
    public class JsonClient
    {
        public string NickName { get; set; }
        public string Login { get; set; }
        public byte[] Logo { get; set; }
        public string? Email { get; set; }
    }
}
