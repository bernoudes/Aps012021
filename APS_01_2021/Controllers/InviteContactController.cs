using APS_01_2021.Models;
using APS_01_2021.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.AspNetCore.SignalR;
using APS_01_2021.Hubs;
using APS_01_2021.Models.ViewModel;
using System.Collections.Generic;
using App.Services.Exceptions;

namespace APS_01_2021.Controllers
{
    public class InviteContactController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly InviteContactService _inviteContactService;

        public InviteContactController(InviteContactService inviteContactService,
            IHubContext<ChatHub> hubContext)
        {
            _inviteContactService = inviteContactService;
            _hubContext = hubContext;
        }


        [HttpGet]
        public IActionResult Create()
        {
            return ViewComponent("InviteContactCreate");
        }

        [HttpPost]
        public async Task<IActionResult> Create(string nickname)
        {
            var userNickname = User.Claims.First().Value;
            MessageBoxViewModel msg = new ();

            if (nickname != null)
            {
                try
                {
                    await _inviteContactService.InsertAsync(userNickname, nickname);

                    FrontAction frontAction = new ("InviteContact","CallInviteContactOpt", userNickname);

                    await _hubContext.Clients.User(nickname).SendAsync("Call", frontAction);

                    return null;
                }
                catch (IntegrityException ex)
                {
                    if(ex.Message == "ContactExists")
                    {
                        msg.Title = "Mensagem para o usuário";
                        msg.Message = "Esse contato já é cadastrado";

                        return ViewComponent("MessageBox", msg);
                    }
                    msg.Title = "Mensagem para o usuário";
                    msg.Message = "Não foi possível enviar convite";

                    return ViewComponent("MessageBox", msg);
                }
            }
            msg.Title = "Mensagem para o usuário";
            msg.Message = "Sem apelido preechido";

            return ViewComponent("MessageBox", msg);
        }


        public IActionResult ListReceive()
        {
            return ViewComponent("InviteContactList");
        }


        public IActionResult GetReceive(string inviterNickName)
        {
            OptionBoxViewModel opt = new ();

            opt.Title = "Você tem um novo convite";
            opt.Message = $"{inviterNickName} quer ser um dos seus contatos";
            opt.OptionsMessage = new List<string> { "Aceitar", "Recusar" };
            opt.MethodReturn = "Accept";
            opt.ControllerReturn = "InviteContact";
            opt.ExtraData = inviterNickName;

            return ViewComponent("OptionBox", opt);
        }

        public async Task<IActionResult> Accept(string selectOption, string extradata)
        {
            if(selectOption == "Aceitar")
            {
                return await RunAccept(extradata, true);
            }
            else
            {
                return await RunAccept(extradata, false);
            }
        }
        
        public async Task<IActionResult> RunAccept(string contactNickName, bool chose) 
        {
            var userNickname = User.Claims.First().Value;

            //chose == true accept; chose == false reject
            if (chose == true)
            {
                try
                {
                    await _inviteContactService.AcceptingAsync(contactNickName, userNickname);
                    return RedirectToAction("Create", "Contact", new { contactNickName = contactNickName });
                    
                }
                catch(Exception)
                {
                    MessageBoxViewModel msg = new ();
                    msg.Title = "Mensagem para o usuário";
                    msg.Message = "Não foi possivél aceitar o novo contato, tente novamente mais tarde";

                    return ViewComponent("MessageBox", msg);
                }
            }
            else
            {
                await _inviteContactService.RejectingAsync(contactNickName, userNickname);
                return null;
            }
        }
    }
}
