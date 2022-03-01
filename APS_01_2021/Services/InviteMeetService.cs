using APS_01_2021.Data;
using APS_01_2021.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace APS_01_2021.Services
{
    public class InviteMeetService
    {
        private MyDbContext _context;
        private UserService _userServices;

        public InviteMeetService( MyDbContext context, UserService userServices)
        {
            _context = context;
            _userServices = userServices;
        }

        public async Task<InviteMeetModel> FindByIdAsync(int id)
        {
            return await _context.InviteMeeting.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<InviteMeetModel>> FindAllByNickName(string nickName)
        {
            var userid = await _userServices.FindIdByNickName(nickName);

            var listInvite = await _context.InviteMeeting
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
