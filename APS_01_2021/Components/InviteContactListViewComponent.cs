using APS_01_2021.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace APS_01_2021.Components
{
    public class InviteContactListViewComponent : ViewComponent
    {
        private readonly InviteContactService _invite;

        public InviteContactListViewComponent(InviteContactService invite) 
        {
            _invite = invite;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claims = UserClaimsPrincipal.Claims.First().Value;
            var list = await _invite.FindAllByNickNameAsync(claims);
            return View(list);
        }
    }
}
