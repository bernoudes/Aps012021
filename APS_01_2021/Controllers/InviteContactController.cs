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
        private UserService _userService;
        private InviteContactService _inviteContactService;

        public InviteContactController(UserService userService, InviteContactService inviteContactService)
        {

            _userService = userService;
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

            var invite = await GetUserAndContactId(contactNickName);

            if (invite == null)
            {
                message = "Usuário Não encontrado";
            }
            else
            {
                try
                {
                    await _inviteContactService.InsertAsync(invite);
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
        [HttpPost]
        public async Task<IActionResult> Accept(string contactNickName)
        {
            return await AcceptAuxiliary(contactNickName, true);
        }
        [HttpPost]
        public async Task<IActionResult> Reject(string contactNickName)
        {
            return await AcceptAuxiliary(contactNickName, false);
        }

        private async Task<IActionResult> AcceptAuxiliary(string nickname,bool type)
        {
            var message = "none";
            var invite = await GetUserAndContactId(nickname);

            if (invite == null)
            {
                message = "Erro, Contate o serviço n°....";
            }
            else
            {
                message = await _inviteContactService.Accepting(invite, type);
            }

            return Json(new { message = message });
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
        private async Task<InviteContactModel> GetUserAndContactId(string contactNickName)
        {
            var userNickname = User.Claims.First().Value;
            var userid = await _userService.FindIdByNickName(userNickname);
            var contactid = await _userService.FindIdByNickName(contactNickName);
            var inviteContact = new InviteContactModel { ContactOneId = contactid, ContactTwoId = userid };

            if(userid == 0 || contactid == 0)
            {
                return null;
            }

            return inviteContact;
        }
    }
}
