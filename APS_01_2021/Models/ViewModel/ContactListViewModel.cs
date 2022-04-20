using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace APS_01_2021.Models.ViewModel
{
    [NotMapped]
    public class ContactListViewModel
    {
        public string ContactNickName { get; set; }
        public string StatusConnection { get; set; } //ONLINE, OFFLINE, BUSY, ABSENT
        public int OrderInUserList { get; set; }
        public bool IsWaitReadMessage { get; set; }


        public ContactListViewModel()
        {
        }

        public ContactListViewModel(ContactModel contModel, int userid)
        {
            ConvertContactForContactList(contModel, userid);
        }

        public void ConvertContactForContactList(ContactModel contModel, int userid)
        {
            bool isWaitReadMes = false;

            if (contModel.UserOneId == userid)
            {
                isWaitReadMes = !contModel.IsUserOneReadMessage;
            }
            else if (contModel.UserTwoId == userid)
            {
                isWaitReadMes = !contModel.IsUserTwoReadMessage;
            }

            ContactNickName = contModel.ContactNickName;
            StatusConnection = contModel.StatusConnection;
            OrderInUserList = contModel.OrderInUserList;
            IsWaitReadMessage = isWaitReadMes;
        }
    }
}
