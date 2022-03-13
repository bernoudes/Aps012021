using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models
{
    [Table("MESSAGE")]
    public class ChatMessageModel
    {
        public int Id { get; set; }
        [Column("USER_ID")]
        public int userId { get; set; }
        [Column("RECEIVE_ID")]
        public int receiverId { get; set; }//contact id or meet id
        [Column("TYPE_RECEIVER")]
        public string type { get; set; } //CONTACT OR MEET
        [Column("MESSAGE")]
        public string message { get; set; }
        [Column("DT_REF")]
        public DateTime time { get; set; }
        [NotMapped]
        public string WhoSendMessage { get; set; }

    }
}
