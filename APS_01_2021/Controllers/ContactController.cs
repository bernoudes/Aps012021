using APS_01_2021.Hubs;
using APS_01_2021.Models;
using APS_01_2021.Models.ViewModel;
using APS_01_2021.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APS_01_2021.Controllers
{
    public class ContactController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ContactService _contactService;
        
        public ContactController(ContactService contactService,
            IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
            _contactService = contactService;
        }

   
        public async Task<ActionResult> Create(string contactNickName)
        {
            var userNickname = User.Claims.First().Value;
            MessageBoxViewModel msg = new ();

            try
            {
                await _contactService.InsertAsync(userNickname, contactNickName);

                var userconn = await _contactService.FindStatusConncectionAsync(userNickname);
                var contactconn = await _contactService.FindStatusConncectionAsync(contactNickName);

                FrontAction frontActionUser = new ("Contact", "AddListContact", new { contact = contactNickName, statusconn = contactconn});
                FrontAction frontActionCont = new ("Contact", "AddListContact", new { contact = userNickname, statusconn = userconn});

                await _hubContext.Clients.User(userNickname).SendAsync("Update", frontActionUser);
                await _hubContext.Clients.User(contactNickName).SendAsync("Update", frontActionCont);

                return null;
            }
            catch (Exception)
            {
                msg.Title = "Mensagem para o usuário";
                msg.Message = "Não foi possivél aceitar o novo contato, tente novamente mais tarde";

                return ViewComponent("MessageBox", msg);
            }
        }


        public async Task<List<ContactModel>> GetList()
        {
            var userNickname = User.Claims.First().Value;
            var list = await _contactService.FindAllByNickNameAsync(userNickname);
            return list;
        }


        [HttpGet]
        public ActionResult Delete(string receiver)
        {
            MessageBoxViewModel msg = new ();

            if (receiver == null)
            {
                msg.Title = "Mensagem para o usuário";
                msg.Message = "Contato não selecionado";

                return ViewComponent("MessageBox", msg);
            }
            else
            {
                OptionBoxViewModel opt = new ()
                {
                    Title = "Deletar Contato",
                    Message = $"Deseja Realmente Deletar o {receiver} ?",
                    OptionsMessage = new List<string>() { "Sim" },
                    ControllerReturn = "Contact",
                    MethodReturn = "DeletePo",
                    ExtraData = receiver 
                };

                return ViewComponent("OptionBox", opt);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Delete(string selectOption, string extraData)
        {
            if(selectOption == "Sim")
            {
                 var userNickname = User.Claims.First().Value;
                 await _contactService.DeleteAsync(userNickname, extraData);

                MessageBoxViewModel msg = new()
                {
                    Title = "Mensagem para o usuário",
                    Message = "Contato não selecionado"
                };

                FrontAction frontActionUser = new ("Contact", "DelListContact", new { nickname = extraData });
                FrontAction frontActionCont = new ("Contact", "DelListContact", new { nickname = userNickname });

                await _hubContext.Clients.User(userNickname).SendAsync("Update", frontActionUser);
                await _hubContext.Clients.User(extraData).SendAsync("Update", frontActionCont);

                return ViewComponent("MessageBox", msg);
            }

            return null;
        }
    }
}
