using APS_01_2021.Data;
using APS_01_2021.Models;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace APS_01_2021.Services
{
    public class InviteListContactServices
    {
        private MyDbContext _context;
        private UserServices _userservice;

        public InviteListContactServices(MyDbContext context, UserServices userservice)
        {
            _context = context;
            _userservice = userservice;
        }
        
        public async Task InsertByNickName(UserModel user, string contactnickname)
        {
            if(user != null && user.NickName != null && contactnickname != null)
            {
                var contactid = await _userservice.FindIdByNickName(contactnickname);
                var userid = await _userservice.FindIdByNickName(user.NickName);
                if (contactid != 0 && userid != 0)
                {
                    InviteListContactModel inviteListContact =
                        new InviteListContactModel() {
                            ContactOneId = userid,
                            ContactTwoId = contactid,
                            DateReference = DateTime.Now
                        };
                    _context.Add(inviteListContact);
                    await _context.SaveChangesAsync();
                }
            }
        }

        /*FIND METHODS*/
        public async Task<List<InviteListContactModel>> FindAllByNickName(string nickname)
        {
            var userid = await _userservice.FindIdByNickName(nickname);
            var listUsers =  await _context.InviteListContact
                .Where(x => x.ContactTwoId == (int) userid && x.IsAccept == "NOT_RESP")
                .ToListAsync();

            foreach(var item in listUsers)
            {
                item.ContactOneNickName = await _userservice.FindNickNameById(item.ContactOneId);
            }

            return listUsers;
        }

    }
}
