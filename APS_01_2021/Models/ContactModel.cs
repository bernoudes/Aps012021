using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models
{
    [Table("CONTACT")]
    public class ContactModel
    {
        public int Id { get; set; }
        [Column("USER_ONE_ID")]
        public int UserOneId { get; set; }
        [Column("USER_TWO_ID")]
        public int UserTwoId { get; set; }
        [Column("USER_ONE_READ_MESSAGE")]
        public bool IsUserOneReadMessage { get; set; }
        [Column("USER_TWO_READ_MESSAGE")]
        public bool IsUserTwoReadMessage { get; set; }
        [Column("USER_ONE_PLACE_LIST")]
        public int UserOnePlaceInTheList { get; set; }
        [Column("USER_TWO_PLACE_LIST")]
        public int UserTwoPlaceInTheList { get; set; }
        [NotMapped]
        public string StatusConnection { get; set; } //ONLINE, OFFLINE, BUSY, ABSENT
        [NotMapped]
        public bool IsSelected { get; set; } = false;
        [NotMapped]
        public string ContactNickName { get; set; }
        [NotMapped]
        public int OrderInUserList { get; set; }
    }
}
