using ModelLibrary.DbModels;
namespace ModelLibrary.DbModels
{
    public class Client
    {
        public Client() 
        {
            SentMessages = new HashSet<DbMessage>();
            ReceivedMessages = new HashSet<DbMessage>();
        }
        public int Id { get; set; }
        public string NickName { get; set; }
        public string Login { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public byte[]? Logo { get; set; }
        public virtual ICollection<DbMessage> SentMessages { get; set; }
        public virtual ICollection<DbMessage> ReceivedMessages { get; set; }
    }
}