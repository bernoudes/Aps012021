using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APS_01_2021.Components
{
    public class MessageViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string title, string message)
        {
            ViewData["Title"] = title;
            ViewData["Message"] = message;
            return View();
        }
    }
}
