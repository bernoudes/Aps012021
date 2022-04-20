using App.Services.Exceptions;
using APS_01_2021.Data;
using APS_01_2021.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace APS_01_2021.Services
{
    public class ContactMeetService
    {
        private readonly MyDbContext _context;
        private readonly UserService _userService;

        public ContactMeetService(MyDbContext context, InviteMeetService inviteMeetService,
            UserService userService, ContactService contactService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task InsertAsync(ContactMeetModel contactMeet)
        {
            try
            {
                if (contactMeet == null)
                    throw new ArgumentNullException("ContactMeetNull");

                _context.ContactMeet.Add(contactMeet);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task UpdateAsync(ContactMeetModel contactMeet)
        {
            try
            {
                _context.ContactMeet.Update(contactMeet);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task DeleteAsync(ContactMeetModel contactMeet)
        {
            try
            {
                _context.ContactMeet.Remove(contactMeet);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }


        //--------------------------------------------------------------------

        public async Task<List<ContactMeetModel>> FindAllByNickNameAsync(string user)
        {
            try
            {
                var userid = await _userService.FindIdByNickName(user);
                return await _context.ContactMeet.Where(x => x.ContactId == userid).ToListAsync();
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        //----------------------------------------------------------------------------
        //below this is the private methods who serve the crud

    }
}
