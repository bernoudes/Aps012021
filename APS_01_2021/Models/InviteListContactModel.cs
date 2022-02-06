using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models
{
    [Table("CONTACT_INVITE")]
    public class InviteListContactModel
    {
        public int Id { get; set; }
        [Column("CONTACT_ONE_ID")]
        public int ContactOneId { get; set; }
        [Column("CONTACT_TWO_ID")]
        public int ContactTwoId { get; set; }
        [Column("DT_REF")]
        public DateTime DateReference { get; set; }

        [NotMapped]
        public string ContactTwoNickName { get; set; }
    }
}
