using APS_01_2021.Data;
using APS_01_2021.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace APS_01_2021.Services
{
    public class ContactService
    {
        private MyDbContext _context;
        private UserService _userServices;

        public ContactService(MyDbContext context, UserService userServices)
        {
            _context = context;
            _userServices = userServices;
        }

        /*create*/
        public async Task<string> InsertAsync(ContactModel contact)
        {
            if (contact == null || contact.UserOneId == 0 || contact.UserTwoId == 0)
            {
                return "PARAMETER_ERROR";
            }

            /*checking existence*/
            var contactResult = await _context.Contact
                .Where(x => x.UserOneId == contact.UserOneId || x.UserOneId == contact.UserTwoId)
                .Where(x => x.UserTwoId == contact.UserOneId || x.UserTwoId == contact.UserTwoId)
                .FirstOrDefaultAsync();

            if (contactResult == null)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return "OK";
            }

            return "CONTACT_EXISTS";
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
                    item.ContactNickName = await _userServices.FindNickNameById(item.UserOneId);
                }
            }

            return list;
        }
    }
}
