using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models
{
    [Table("INVITE_CONTACT")]
    public class InviteContactModel
    {
        public int Id { get; set; }
        [Column("INVITER_ID")]
        public int Inviter_Id { get; set; }
        [Column("INVITED_ID")]
        public int Invited_Id { get; set; }
        [Column("DT_REF")]
        public DateTime DateReference { get; set; }
        [Column("IS_ACCEPT")]
        public string IsAccept { get; set; }

        [NotMapped]
        public string Invited_NickName { get; set; }
    }
}
