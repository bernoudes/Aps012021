using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models.ViewModel
{
    [NotMapped]
    public class InviteMeetViewModel
    {
        public DateTime MeetingDate { get; set; }
        public TimeSpan MeetingTime { get; set; }
        public List<UserAcceptViewModel> ContactList { get; set; }
    }
}
