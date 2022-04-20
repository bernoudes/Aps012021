using APS_01_2021.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APS_01_2021.Components
{
    public class MeetCreateViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(MeetModel meetModel)
        {
            return View(meetModel);
        }
    }
}
