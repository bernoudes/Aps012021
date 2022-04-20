using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models
{
    [Table("CONTACT_MEETING")]
    public class ContactMeetModel
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("MEET_ID")]
        public int IdMeet { get; set; }
        [Column("CONTACT_ID")]
        public int ContactId { get; set; }
        [Column("IS_MODERATOR")]
        public bool IsModerator { get; set; }
        [Column("DT_REF")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public void ConvertInviteMeetForContactMeet(InviteMeetModel inviteMeet)
        {
            IdMeet = inviteMeet.MeetingId;
            ContactId = inviteMeet.ContactId;
            IsModerator = inviteMeet.IsModerator;
        }
    }
}
