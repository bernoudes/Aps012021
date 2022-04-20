using APS_01_2021.Data;
using APS_01_2021.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using App.Services.Exceptions;
using System;

namespace APS_01_2021.Services
{
    public class InviteMeetService
    {
        private readonly MyDbContext _context;
        private readonly UserService _userServices;

        public InviteMeetService( MyDbContext context, UserService userServices)
        {
            _context = context;
            _userServices = userServices;
        }

        public async Task InsertAsync(InviteMeetModel inviteModel)
        {
            try
            {
                if(inviteModel == null)
                    throw new IntegrityException("Obj_Not_Found");

                _context.InviteMeeting.Add(inviteModel);
                await _context.SaveChangesAsync();
            }catch(Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task InsertRangeAsync(List<InviteMeetModel> liInviteModel)
        {
            try
            {
                if (liInviteModel == null || liInviteModel.Count <= 0)
                    throw new IntegrityException("Obj_Not_Found");

                _context.InviteMeeting.AddRange(liInviteModel);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task UpdateAsync(InviteMeetModel inviteModel)
        {
            try
            {
                if (inviteModel == null || inviteModel.Id == 0)
                    throw new IntegrityException("Obj_Not_Found");

                _context.InviteMeeting.Update(inviteModel);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task DeleteAsync(InviteMeetModel inviteModel)
        {
            try
            {
                if (inviteModel == null || inviteModel.Id == 0)
                    throw new IntegrityException("Obj_Not_Found");

                _context.InviteMeeting.Remove(inviteModel);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }


        //--------------------------------------------------------------------
        public async Task<InviteMeetModel> FindByIdAsync(int id)
        {
            try
            {
                return await _context.InviteMeeting.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task<InviteMeetModel> FindByMeetIdAndNickNameAsync(int idMeet, string user)
        {
            try
            {
                var userid = await _userServices.FindIdByNickName(user);

                return await _context.InviteMeeting
                    .Where(x => x.ContactId == userid)
                    .FirstOrDefaultAsync(x => x.MeetingId == idMeet);
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public async Task<List<InviteMeetModel>> FindAllByNickName(string user)
        {
            try
            {
                var userid = await _userServices.FindIdByNickName(user);

                var listInvite = await _context.InviteMeeting
                    .Where(x => x.Id == (int)userid && x.IsAccept == "NOT_RESP")
                    .ToListAsync();

                foreach (var item in listInvite)
                {
                    //HERE IS NEED TO GET MEETING DATE
                    item.ContactId = (int)userid;
                    item.AdminUserNickName = await _userServices.FindNickNameById(item.AdmUserId);
                }

                return listInvite;
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }
        //--------------------------------------------------------------------
        //below this is the methods with different uses
        public async Task AcceptInviteAsync(int idMeet, string user)
        {
            await AcceptingOrRejectingInviteAsync(idMeet,user, true);
        }


        public async Task RejectInviteAsync(int idMeet, string user)
        {
            await AcceptingOrRejectingInviteAsync(idMeet, user, false);
        }



        //----------------------------------------------------------------------------
        //below this is the private methods who serve the crud
        private async Task AcceptingOrRejectingInviteAsync(int idMeet, string user, bool chose)
        {
            //chose == true : accept
            //chose == false : reject

            try
            {
                var inviteMeetModel = await FindByMeetIdAndNickNameAsync(idMeet, user);

                if ( inviteMeetModel != null )
                {
                    inviteMeetModel.IsAccept = chose == true ? "YES" : "NO";
                    await UpdateAsync(inviteMeetModel);

                    if( chose == true )
                    {
                        ContactMeetModel contactMeetModel = new();
                        contactMeetModel.ConvertInviteMeetForContactMeet(inviteMeetModel);

                        //await _contactMeetService.InsertAsync(contactMeetModel);
                    }
                }
                else
                {
                    throw new IntegrityException("Obj_Not_Found");
                }
            }
            catch (Exception e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        public List<InviteMeetModel> ConvertMeetModelForInviteMeetModel(MeetModel meetModel, List<int> contactId)
        {
            if (meetModel == null || meetModel.MeetContactConf.Count == 0)
                return null;

            List<InviteMeetModel> liInviteMeet = new();

            for (int i = 0; i < meetModel.MeetContactConf.Count; i++)
            {
                var newInviteMeet = new InviteMeetModel()
                {
                    MeetingId = meetModel.Id,
                    AdmUserId = meetModel.AdmUserId,
                    IsAccept = "NOT_RESP",
                    ContactId = contactId[i],
                    IsModerator = meetModel.MeetContactConf[i].IsModerator,
                    CreationDate = DateTime.Now
                };

                liInviteMeet.Add(newInviteMeet);
            }
            return liInviteMeet;
        }

    }
}
