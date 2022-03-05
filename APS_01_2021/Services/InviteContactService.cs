using APS_01_2021.Data;
using APS_01_2021.Models;
using APS_01_2021.Services;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using App.Services.Exceptions;

namespace APS_01_2021.Services
{
    public class InviteContactService
    {
        private MyDbContext _context;
        private UserService _userservice;
        private ContactService _contactService;

        public InviteContactService(MyDbContext context, UserService userservice, ContactService contactService)
        {
            _context = context;
            _userservice = userservice;
            _contactService = contactService;
        }

        public async Task InsertAsync(InviteContactModel inviteContact)
        {
            var exist = _context.InviteContact
                .Where(x => x.ContactOneId == inviteContact.ContactOneId || x.ContactTwoId == inviteContact.ContactOneId)
                .Where(x => x.ContactOneId == inviteContact.ContactTwoId || x.ContactTwoId == inviteContact.ContactTwoId)
                .FirstOrDefault();

            if (inviteContact != null && exist == null)
            {
                if (inviteContact.ContactOneId != 0 || inviteContact.ContactTwoId != 0)
                {
                    _context.Add(inviteContact);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                throw new Exception("InviteExists");
            }
        }

        /*FIND METHODS*/
        public async Task<List<InviteContactModel>> FindAllByNickName(string nickname)
        {
            var userid = await _userservice.FindIdByNickName(nickname);
            var listUsers = await _context.InviteContact
                .Where(x => x.ContactTwoId == (int)userid && x.IsAccept == "NOT_RESP")
                .ToListAsync();

            foreach (var item in listUsers)
            {
                item.ContactOneNickName = await _userservice.FindNickNameById(item.ContactOneId);
            }

            return listUsers;
        }

        /*UPDATE METHODS*/
        public async Task<string> Accepting(InviteContactModel invite, bool IsAccept)
        {
            var inviteWithId = await _context.InviteContact
                .Where(x => x.IsAccept == "NOT_RESP")
                .Where(x => x.ContactOneId == invite.ContactOneId)
                .Where(x => x.ContactTwoId == invite.ContactTwoId)
                .FirstOrDefaultAsync();

            if (inviteWithId != null)
            {
                if (IsAccept)
                {
                    inviteWithId.IsAccept = "YES";
                    var contact = new ContactModel { 
                        UserOneId = inviteWithId.ContactOneId,
                        UserTwoId = inviteWithId.ContactTwoId
                    };
                    await _contactService.InsertAsync(contact);
                }
                else
                {
                    inviteWithId.IsAccept = "NO";
                }
                try
                {
                    _context.Update(inviteWithId);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return "Error";
                }
            }
            return "Ok";
        }
    }
}
