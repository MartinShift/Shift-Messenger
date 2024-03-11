using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.DbModels
{
    public class DbMessage
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public byte[]? Image { get; set; }
        public string? String { get; set; }
        public int FromClientId { get; set; }
        public int ToClientId { get; set; }
        public virtual Client FromClient { get; set; }
        public virtual Client ToClient { get; set; }
    }
}
