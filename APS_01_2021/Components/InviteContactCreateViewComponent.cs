using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APS_01_2021.Components
{
    public class InviteContactCreateViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string type, string message)
        {
            if (type != null && message != null)
            {
                ViewData[type] = message;
            }
            return View();
        }
    }
}
