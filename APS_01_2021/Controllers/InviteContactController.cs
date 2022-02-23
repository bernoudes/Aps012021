using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APS_01_2021.Controllers
{
    public class InviteContactController : Controller
    {
        public IActionResult Create()
        {
            return ViewComponent("InviteContactCreate");
        }
       /* [HttpPost]
        public async Task<IActionResult> Create()
        {
            return View();
        }*/
        //-----------------------------------------------------
        public IActionResult GetListSend()
        {
            return View();
        }
        public IActionResult GetListReceive()
        {
            return ViewComponent("InviteContactList");
        }
        //------------------------------------------------------
        public IActionResult Delete()
        {
            return ViewComponent("Delete");
        }
        /*[HttpPost]
        public Task<IActionResult> Delete()
        {
            return View();
        }*/
        //------------------------------------------------------

    }
}
