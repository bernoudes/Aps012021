using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using APS_01_2021.Hubs;

namespace APS_01_2021.Controllers
{
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
