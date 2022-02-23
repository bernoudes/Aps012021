using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models
{
    [Table("INVITE_CONTACT")]
    public class InviteContactModel
    {
        public int Id { get; set; }
        [Column("USER_ONE_ID")]
        public int ContactOneId { get; set; }
        [Column("USER_TWO_ID")]
        public int ContactTwoId { get; set; }
        [Column("DT_REF")]
        public DateTime DateReference { get; set; }
        [Column("IS_ACCEPT")]
        public string IsAccept { get; set; }

        [NotMapped]
        public string ContactOneNickName { get; set; }
    }
}
