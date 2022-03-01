using Microsoft.AspNetCore.Mvc;
using APS_01_2021.Models;
using APS_01_2021.Services;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace APS_01_2021.Controllers
{
    public class InviteMeetController : Controller
    {
        public IActionResult Create()
        {
            return ViewComponent("InviteMeetCreate");
        }
        //-----------------------------------------------------
        public IActionResult GetListSend()
        {
            return View();
        }
        public IActionResult GetListReceive()
        {
            return ViewComponent("InviteMeetList");
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
