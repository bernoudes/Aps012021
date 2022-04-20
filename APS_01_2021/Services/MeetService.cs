using App.Services.Exceptions;
using APS_01_2021.Data;
using APS_01_2021.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace APS_01_2021.Services
{
    public class MeetService
    {
        private readonly MyDbContext _context;
        private readonly UserService _userService;
        private readonly ContactService _contactService;

        public MeetService(MyDbContext context,
            UserService userService, ContactService contactService)
        {
            _contactService = contactService;
            _context = context;
            _userService = userService;
        }

        public async Task<MeetModel> InsertAsync(string user,MeetModel meetModel)
        {
            if (meetModel.MeetContactConf.Count != meetModel.MeetContactConf.Count)
                throw new Exception("Lists Don't have the same size");

            try
            {
                meetModel.AdmUserId = await _userService.FindIdByNickName(user);
                meetModel.CreationDate = DateTime.Now;

                _context.Add(meetModel);
                await _context.SaveChangesAsync();

                //------------------------------

                MeetModel newMeetResult = await FindLastByAdmNickNameAsync(user);
                meetModel.Id = newMeetResult.Id;

                return meetModel;
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task UpdateAsync(string user, MeetModel meetModel)
        {
            try
            {
                if (meetModel == null)
                    throw new IntegrityException("Obj_Not_Found");

                var userid = await _userService.FindIdByNickName(user);
                var meetComplet = await FindByIdAndAdmNickNameAsync(user, meetModel.Id);

                _context.Meet.Update(meetModel);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task DeleteAsync(MeetModel meetModel)
        {
            try
            {
                if (meetModel == null)
                    throw new IntegrityException("Obj_Not_Found");

                _context.Meet.Remove(meetModel);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        //--------------------------------------------------------------------

        public async Task<MeetModel> FindLastByAdmNickNameAsync(string user)
        {
            try
            {
                var userid = await _userService.FindIdByNickName(user);
                var meetModel = await _context.Meet.LastOrDefaultAsync(x => x.AdmUserId == userid);
                return meetModel;
            }
            catch(Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task<MeetModel> FindByIdAsync(int meetid)
        {
            try
            {
                return await _context.Meet.FirstOrDefaultAsync(x => x.Id == meetid);
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task<List<MeetModel>> FindAllByAdminNickNameAsync(string user)
        {
            try
            {
                var userid = await _userService.FindIdByNickName(user);
                return await _context.Meet.Where(x => x.AdmUserId == userid).ToListAsync();
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task<MeetModel> FindByIdAndAdmNickNameAsync(string user, int meetId)
        {
            try
            {
                var userid = await _userService.FindIdByNickName(user);

                var meetResult = await _context.Meet
                    .FirstOrDefaultAsync(x => x.AdmUserId == userid && x.Id == meetId);

                if (meetResult == null)
                    throw new IntegrityException("User not the adm");

                return await MeetComplet(meetResult);
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        //----------------------------------------------------------------------------
        //below this is the public methods who serve the controller
        public async Task<MeetModel> NewMeetModel(string user)
        {
            var contacts = await _contactService.FindAllByNickNameAsync(user);

            MeetModel meetModel = new MeetModel();
            meetModel.Id = 0;
            meetModel.MeetingDate = DateTime.Now;
            meetModel.MeetingTime = TimeSpan.Zero;
            meetModel.DescriptionSubject = "";

            foreach(var contact in contacts)
            {
                meetModel.AddMeetContactConf(contact.ContactNickName, false, "none");
            }
 
            return meetModel;
        }

        //----------------------------------------------------------------------------
        //below this is the private methods who serve the crud
        private int ContactResut(int userid,ContactModel contactModel)
        {
            if (contactModel.UserOneId == userid)
                return contactModel.UserTwoId;

            if (contactModel.UserTwoId == userid)
                return contactModel.UserOneId;

            return 0;
        }

        private async Task<MeetModel> MeetComplet(MeetModel meetModel)
        {
            var meetResult = meetModel;
/*var listContactMeet = await _contactMeetService.FindAllByMeetIdAsync(meetResult.Id);
            if (listContactMeet == null)
                throw new IntegrityException("Contact List Return Null");

            meetResult.ContactsStatus = new();
            meetResult.ContactsNickName = new();
            meetResult.ContactIsModerator = new();

            foreach (var item in listContactMeet)
            {
                var contactNick = await _userService.FindNickNameById(item.ContactId);

                meetResult.ContactIsModerator.Add(item.IsModerator);
                meetResult.ContactsNickName.Add(contactNick);
                meetResult.ContactsStatus.Add("none");
            }*/

            return meetResult;
        }
    }
}
