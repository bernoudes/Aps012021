using APS_01_2021.Models;
using APS_01_2021.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace APS_01_2021.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserService _userService;
        private readonly ContactService _contactService;

        public ChatHub(UserService userService, ContactService contactService)
        {
            _userService = userService;
            _contactService = contactService;
        }

        public override async Task OnConnectedAsync()
        {
            await SendUserStatusConnection("online");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await SendUserStatusConnection("offline");
            await base.OnDisconnectedAsync(exception);
        }

        private async Task SendUserStatusConnection(string status)
        {
            var userNickname = Context.User.Claims.First().Value;
           // await _contactService.ReorderListAsync(userNickname);
            var listcontact = await _contactService.FindAllByNickNameAsync(userNickname);

            await _userService.UpdateStatusConnection(userNickname, status);

            FrontAction frontAction = new ("Contact", "UpdateStatusConn", new { nickname = userNickname, status = status.ToUpper() });

            foreach (var contact in listcontact)
            {
                await Clients.User(contact.ContactNickName).SendAsync("Update", frontAction);
            }
        }
    }
}
