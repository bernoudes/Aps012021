using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models
{
    [Table("MEET")]
    public class MeetModel
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("ADM_USER_ID")]
        public int AdmUserId { get; set; }
        [Column("DESCRIPTION_SUBJECT")]
        public string DescriptionSubject { get; set; }
        [Column("DT_MEET")]
        public DateTime MeetDate { get; set; }
        [Column("DT_REF")]
        public DateTime CreationDate { get; set; }


        [NotMapped]
        public DateTime MeetingDate { get; set; }
        [NotMapped]
        public TimeSpan MeetingTime { get; set; }
        [NotMapped]
        public List<MeetContactConfig> MeetContactConf { get; set; } = new();


        public struct MeetContactConfig
        {
            [NotMapped]
            public string NickName { get; set; }

            [NotMapped]
            public bool IsModerator { get; set; }
            [NotMapped]
            public string Status { get; set; }
        }

        public void AddMeetContactConf(string nickname, bool isModerator, string status)
        {
            MeetContactConfig meetconfig = new MeetContactConfig()
            {
                NickName = nickname,
                IsModerator = isModerator,
                Status = status
            };

            MeetContactConf.Add(meetconfig);
        }
    }
}
