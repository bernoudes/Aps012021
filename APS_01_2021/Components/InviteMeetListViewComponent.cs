using APS_01_2021.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace APS_01_2021.Components
{
    public class InviteMeetListViewComponent : ViewComponent
    {
        /*private InviteListMeetingServices _inviteService;

        public InviteListMeetingViewComponent(InviteListMeetingServices inviteService)
        {
            _inviteService = inviteService;
        }*/
        public async Task<IViewComponentResult> InvokeAsync()
        {
            /*var claims = UserClaimsPrincipal.Claims.First().Value;
            var list = _inviteService.FindAllByNickName(claims);
            return View(list);*/
            return View();
        }
    }
}
