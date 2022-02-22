using APS_01_2021.Data;
using APS_01_2021.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace APS_01_2021.Services
{
    public class ContactServices
    {
        private MyDbContext _context;
        private UserServices _userServices;

        public ContactServices(MyDbContext context, UserServices userServices)
        {
            _context = context;
            _userServices = userServices;
        }

        public async Task<List<ContactModel>> FindAllByNickName(string nickName)
        {
            var userid = await _userServices.FindIdByNickName(nickName);
            var list = await _context.Contact
                .Where(x => x.UserOneId == userid || x.UserTwoId == userid)
                .ToListAsync();

            foreach(var item in list)
            {   
                if(item.UserOneId == userid)
                {
                    item.ContactNickName = await _userServices.FindNickNameById(item.UserTwoId);
                }
                else
                {
                    item.ContactNickName = await _userServices.FindNickNameById(item.UserTwoId);
                }
            }

            return list;
        }
    }
}
