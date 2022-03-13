using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models.ViewModel
{
    [NotMapped]
    public class MessageForChatViewModel
    {
        public string WhoSendMessage { get; set; }
        public string Message { get; set; }
        public string Time { get; set; }
    }
}
