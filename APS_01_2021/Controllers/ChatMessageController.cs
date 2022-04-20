using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using APS_01_2021.Hubs;
using System.Threading.Tasks;
using APS_01_2021.Models;
using System.Linq;
using APS_01_2021.Services;
using System;
using System.Collections.Generic;
using APS_01_2021.Models.ViewModel;

namespace APS_01_2021.Controllers
{
    public class ChatMessageController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private ChatMessageService _chatService;
        private UserService _userService;

        public ChatMessageController(
            IHubContext<ChatHub> hubContext, UserService userService,
            ChatMessageService chatService)
        {
            _hubContext = hubContext;
            _chatService = chatService;
            _userService = userService;
        }

        public async Task<string> SendMessage(string type, string receiver, string message)
        {
            var userNickname = User.Claims.First().Value;
            var userid = await UserId();
            var receiverid = 0;

            if (type == "contact")
            {
                receiverid = await _userService.FindIdByNickName(receiver);
            }
            else if (type == "meet")
            {
                //receiverid = await _meetService.FindIdByKeyName(receiver);
            }

            if(receiverid != 0) { 
                var chatMessage = new ChatMessageModel()
                {
                    userId = userid,
                    type = type,
                    receiverId = receiverid,
                    message = message,
                    time = DateTime.Now,
                    WhoSendMessage = userNickname
                };
                await _chatService.InsertAsync(chatMessage);
              
                FrontAction frontAction = new(
                    "ChatMessage", "ReceiveMessage",
                    _chatService.ConvertChatMessageForMessageViewModel(chatMessage));

                await _hubContext.Clients.User(receiver).SendAsync("Update", frontAction);
                await _hubContext.Clients.User(userNickname).SendAsync("Update", frontAction);
            }
            return "OK";
        }

        [HttpPost]
        public async Task UserSeeTheMessage(string type, string receiver)
        {
            var userid = await UserId();
            if(receiver != null)
            {
                await _chatService.UserReadTheMessageAsync(type, userid, receiver);
            }
        }


        public async Task<string> GetConversation(string type, string receiver)
        {
            var userNickname = User.Claims.First().Value;
            var userid = await UserId();
            var receiverid = 0;
            var list = new List<MessageForChatViewModel>();

            if (type == "contact")
            {
                receiverid = await _userService.FindIdByNickName(receiver);
                list = await _chatService.GetAllMessageContact(userid, receiverid, userNickname, receiver);
            }
            else if (type == "meet")
            {
                //receiverid = await _meetService.FindIdByKeyName(receiver);
            }

            FrontAction frontAction = new(
                "ChatMessage", "ReceiveFullChat", list);

            await _hubContext.Clients.User(userNickname).SendAsync("Update", frontAction);

            return "OK";
        }

        private async Task<int> UserId()
        {
            var userNickname = User.Claims.First().Value;
            var userid = await _userService.FindIdByNickName(userNickname);
            return userid;
        }
    }
}
