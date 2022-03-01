using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models
{
    [Table("MEETING_INVITE")]
    public class InviteMeetModel
    {
        public int Id { get; set; }
        [Column("MEETING_ID")]
        public int MeetingId { get; set; }
        [Column("ADMIN_USER_ID")]
        public int AdmUserId { get; set; }
        [Column("CONTACT_ID")]
        public int ContactId { get; set; }
        [Column("IS_ACCEPT")]
        public string IsAccept { get; set; }

        [NotMapped]
        public string AdminUserNickName { get; set; }
        [NotMapped]
        public DateTime Meeting_Time { get; set; }
    }
}
