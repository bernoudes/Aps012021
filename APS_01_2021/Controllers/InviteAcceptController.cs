using APS_01_2021.Services;
using Microsoft.AspNetCore.Mvc;

namespace APS_01_2021.Controllers
{
    public class InviteAcceptController : Controller
    {
        private InviteContactService _inviteContact;
        private InviteMeetService _inviteMeet;
        public IActionResult GetAll()
        {
            return View();
        }
    }
}
