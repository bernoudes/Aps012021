using APS_01_2021.Models;
using APS_01_2021.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace APS_01_2021.Controllers
{
    public class InviteContactController : Controller
    {
        private UserService _userServise;
        private InviteContactService _inviteContactService;

        public InviteContactController(UserService userService, InviteContactService inviteContactService)
        {

            _userServise = userService;
            _inviteContactService = inviteContactService;
        }

        public IActionResult Create()
        {
            return ViewComponent("InviteContactCreate");
        }
        [HttpPost]
        public async Task<IActionResult> Create(string contactNickName)
        {
            string message = "Nhc";

            var userNickname = User.Claims.First().Value;
            var userid =  await _userServise.FindIdByNickName(userNickname);
            var contactid = await _userServise.FindIdByNickName(contactNickName);
            var inviteContact = new InviteContactModel { ContactOneId = userid, ContactTwoId = contactid };

            if (userid == 0 || contactid == 0)
            {
                message = "Usuário Não encontrado";
            }
            else
            {
                try
                {
                    await _inviteContactService.InsertAsync(inviteContact);
                    message = "Convite Enviado";
                }
                catch(Exception)
                {
                    message = "Convite já existente";
                }
            }

            return Json(new { message = message });
        }
        //-----------------------------------------------------
        public IActionResult GetListSend()
        {
            return View();
        }
        public IActionResult GetListReceive()
        {
            return ViewComponent("InviteContactList");
        }
        //------------------------------------------------------
        public IActionResult Delete()
        {
            return ViewComponent("Delete");
        }
        /*[HttpPost]
        public Task<IActionResult> Delete()
        {
            return View();
        }*/
        //------------------------------------------------------

    }
}
