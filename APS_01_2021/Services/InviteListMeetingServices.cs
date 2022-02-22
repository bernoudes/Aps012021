using APS_01_2021.Data;
using APS_01_2021.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace APS_01_2021.Services
{
    public class InviteListMeetingServices
    {
        private MyDbContext _context;
        private UserServices _userServices;

        public InviteListMeetingServices( MyDbContext context, UserServices userServices)
        {
            _context = context;
            _userServices = userServices;
        }

        public async Task<InviteListMeetingModel> FindByIdAsync(int id)
        {
            return await _context.InviteListMeeting.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<InviteListMeetingModel>> FindAllByNickName(string nickName)
        {
            var userid = await _userServices.FindIdByNickName(nickName);

            var listInvite = await _context.InviteListMeeting
                .Where(x => x.Id == (int)userid && x.IsAccept == "NOT_RESP")
                .ToListAsync();

            foreach(var item in listInvite)
            {
                //HERE IS NEED TO GET MEETING DATE
                item.ContactId = (int)userid;
                item.AdminUserNickName = await _userServices.FindNickNameById(item.AdmUserId);
            }

            return listInvite;
        }

    }
}
