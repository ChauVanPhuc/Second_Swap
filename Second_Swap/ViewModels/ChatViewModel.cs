

using Second_Swap.Models;

namespace Second_Swap.ViewModels
{
    public class ChatViewModel
    {
        public string RecipientName { get; set; }
        public int revId { get; set; }
        public int sendvId { get; set; }
        public string read { get; set; }
        public string avatar { get; set; }
        public int countMeess { get; set; }
        public List<Message> MyMessages { get; set; }
        public List<Message> OtherMessages { get; set; }
        public Message LastMessage { get; set; }
    }
}
