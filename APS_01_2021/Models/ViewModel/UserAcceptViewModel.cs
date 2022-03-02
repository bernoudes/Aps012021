using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models.ViewModel
{
    [NotMapped]
    public class UserAcceptViewModel
    {
        public bool Invited { get; set; }
        public string UserNickName { get; set; }
    }
}
