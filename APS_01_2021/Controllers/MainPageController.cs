using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APS_01_2021.Models
{
    public class MainPageController : Controller
    {
        public IActionResult Chat()
        {
            return View();
        }
    }
}
