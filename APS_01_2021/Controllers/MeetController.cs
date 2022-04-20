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
    public class MeetController : Controller
    {
        private readonly MeetService _meetService;
        private readonly ContactService _contactService;
        private readonly IHubContext<ChatHub> _hubContext;

        public MeetController(MeetService meetService, IHubContext<ChatHub> hubContext, ContactService contactService)
        {
            _meetService = meetService;
            _hubContext = hubContext;
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userNickname = User.Claims.First().Value;
            MeetModel meetModel = await _meetService.NewMeetModel(userNickname);

            return ViewComponent("MeetCreate", meetModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MeetModel meetModel)
        {
            var userNickname = User.Claims.First().Value;
            MessageBoxViewModel msg = new();

            if (userNickname != null && meetModel != null)
            {
                try
                {
                    if(meetModel.MeetContactConf.Count > 0)
                    {
                        var meetModelComplet = await _meetService.InsertAsync(userNickname, meetModel);
                        return RedirectToAction("Create", "InviteContact", meetModelComplet);
                    }
                }
                catch(Exception e)
                {
                    msg.Title = "Error de Argumentos";
                    msg.Message = "Por alguma razão não foi possível registrar uma nova reunião";

                    return ViewComponent("MessageBox", msg);
                }
            }

            msg.Title = "Error de Argumentos";
            msg.Message = "Por alguma razão não foi possível registrar uma nova reunião";

            return ViewComponent("MessageBox", msg);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int meetId)
        {
            var userNickname = User.Claims.First().Value;
            MessageBoxViewModel msg = new();

            try
            {
                //this only work if the user is the adm
                MeetModel meetEdit = await _meetService.FindByIdAndAdmNickNameAsync(userNickname, meetId);
                return ViewComponent("MeetCreate", meetEdit);
            }
            catch(Exception e)
            {
                msg.Title = "Error de Argumentos";
                msg.Message = "Por alguma razão não foi possível abrir o editor";

                return ViewComponent("MessageBox", msg);
            } 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MeetModel meet)
        {
            var userNickname = User.Claims.First().Value;
            MessageBoxViewModel msg = new();

            try
            {
                //this only work if the user is the adm
                await _meetService.UpdateAsync(userNickname, meet);

                msg.Title = "Sucesso";
                msg.Message = "A alteração foi realizada com sucesso";

                return ViewComponent("MessageBox", msg);
            }
            catch (Exception e)
            {
                msg.Title = "Error de Argumentos";
                msg.Message = "Não foi possível gravar a edição";

                return ViewComponent("MessageBox", msg);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Remove(int meetId)
        {
            var userNickname = User.Claims.First().Value;
            MessageBoxViewModel msg = new();
            OptionBoxViewModel optionBox = new();

            try
            {
                MeetModel meet = await _meetService.FindByIdAndAdmNickNameAsync(userNickname, meetId);

                optionBox.Title = "Deletando Reunião";
                optionBox.Message = $"Deseja realmente deletar essa reunião ?\nReunião: {meet.DescriptionSubject}";
                optionBox.OptionsMessage = new List<string>() { "Aceitar", "Rejeitar" };
                optionBox.UrlReturn = "Remove/Meet";

                return ViewComponent("OptionBox", optionBox);
            }
            catch (Exception e)
            {
                msg.Title = "Error de Argumentos";
                msg.Message = "Não foi possível deletar";

                return ViewComponent("MessageBox", msg);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remove(MeetModel meet)
        {
            var userNickname = User.Claims.First().Value;
            MessageBoxViewModel msg = new();

            try
            {
                var meetComplet = await _meetService.FindByIdAndAdmNickNameAsync(userNickname,meet.Id);
                await _meetService.DeleteAsync(meetComplet);

                //Send to all contacts who have accepted
                FrontAction frontAction = new ("Meet", "Delete", meetComplet.Id);

                foreach(var meetConfig in meetComplet.MeetContactConf)
                {
                    await _hubContext.Clients.User(meetConfig.NickName).SendAsync("Update", frontAction);
                }

                msg.Title = "Reunião";
                msg.Message = "Reunião deletada com sucesso";

                return ViewComponent("MessageBox", msg);
            }
            catch (Exception e)
            {
                msg.Title = "Error de Argumentos";
                msg.Message = "Não foi possível deletar";

                return ViewComponent("MessageBox", msg);
            }
        }
    }
}
