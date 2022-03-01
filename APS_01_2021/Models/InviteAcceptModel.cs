﻿using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models
{
    [NotMapped]
    public class InviteAcceptModel
    {
        public InviteContactModel inviteContact { get; set; }
        public InviteMeetModel inviteMeeting { get; set; }
    }
}
