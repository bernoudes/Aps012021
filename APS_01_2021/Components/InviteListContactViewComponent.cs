using APS_01_2021.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace APS_01_2021.Components
{
    public class InviteListContactViewComponent : ViewComponent
    {
        private readonly InviteListContactServices _invite;

        public InviteListContactViewComponent(InviteListContactServices invite) 
        {
            _invite = invite;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claims = UserClaimsPrincipal.Claims.First().Value;
            var list = await _invite.FindAllByNickName(claims);
            return View(list);
        }
    }
}
