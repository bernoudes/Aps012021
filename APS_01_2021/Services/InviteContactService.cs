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

        public async Task InsertAsync(string user, string contact)
        {
            var userid = await _userservice.FindIdByNickName(user);
            var contactid = await _userservice.FindIdByNickName(contact);
            var contactobj = await _contactService.FindByNickNamesAsync(user, contact);

            var exist = _context.InviteContact
                .Where(x => x.Inviter_Id == userid || x.Inviter_Id == contactid)
                .Where(x => x.Invited_Id == contactid || x.Invited_Id == userid)
                .FirstOrDefault();

            if(contactobj != null)
            {
                throw new IntegrityException("ContactExists");
            }
            else if(exist == null || exist.IsAccept != "NOT_RESP" )
            {
                try
                {
                    InviteContactModel invmodel = new InviteContactModel()
                    {
                        Inviter_Id = userid,
                        Invited_Id = contactid,
                        IsAccept = "NOT_RESP",
                        DateReference = DateTime.Now
                    };

                    _context.Add(invmodel);
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    throw new IntegrityException(ex.Message);
                }
            }
        }

        /*FIND METHODS*/
        public async Task<List<InviteContactModel>> FindAllByNickNameAsync(string user)
        {
            var userid = await _userservice.FindIdByNickName(user);
            var listUsers = await _context.InviteContact
                .Where(x => x.Invited_Id == (int)userid && x.IsAccept == "NOT_RESP")
                .ToListAsync();

            foreach (var item in listUsers)
            {
                item.Invited_NickName = await _userservice.FindNickNameById(item.Inviter_Id);
            }

            return listUsers;
        }

        /*UPDATE METHODS*/
        public async Task<string> AcceptingAsync(string inviter, string invited)
        {
            return await AcceptingChoseAsync(inviter, invited, true);
        }

        public async Task<string> RejectingAsync(string inviter, string invited)
        {
            return await AcceptingChoseAsync(inviter, invited, false);
        }

        private async Task<string> AcceptingChoseAsync(string inviter, string invited,bool chose)
        {
            var inviterId = await _userservice.FindIdByNickName(inviter);
            var invitedId = await _userservice.FindIdByNickName(invited);

            var inviteWithId = await _context.InviteContact
                .Where(x => x.IsAccept == "NOT_RESP")
                .Where(x => x.Inviter_Id == inviterId)
                .Where(x => x.Invited_Id == invitedId)
                .FirstOrDefaultAsync();
            
            if(inviteWithId != null)
            {
                try
                {
                    inviteWithId.IsAccept = chose == true ? "YES" : "NO";
                    _context.Update(inviteWithId);
                    await _context.SaveChangesAsync();
                } 
                catch(Exception ex)
                {
                    throw new IntegrityException(ex.Message);
                }
            }
            else
            {
                return "Invite not found";
            }

            return "OK";
        }
    }
}
