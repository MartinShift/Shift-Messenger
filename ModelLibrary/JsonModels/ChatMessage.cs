
namespace ModelLibrary.JsonModels
{
    public class ChatMessage
    {
        public JsonClient From { get; set; } 
        public JsonClient To { get; set; } 
        public DateTime Date { get; set; }
        public byte[]? Image { get; set; }
        public string? String { get; set; }
    }
}
