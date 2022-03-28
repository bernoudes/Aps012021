using APS_01_2021.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APS_01_2021.Components
{
    public class OptionBoxViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(OptionBoxViewModel opitionBox)
        {
            return View(opitionBox);
        }
    }
}
