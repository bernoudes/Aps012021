using App.Services.Exceptions;
using APS_01_2021.Models;
using APS_01_2021.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APS_01_2021.Controllers
{
    public class UserController : Controller
    {
        private readonly UserServices _userService;

        public UserController(UserServices userService)
        {
            _userService = userService; 
        }

        //LOGIN AREA
        public IActionResult Login()
        {
            return View();
        }
        //REGISTER AREA
        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserModel user)
        {
            if (user == null)
            {
                TempData["Error"] = "Faltando Informação";
                return View("Register");
            }
            try
            {
                await _userService.InsertAsync(user);
                return View("Register");
            }
            catch (IntegrityException ex)
            {
                TempData["Error"] = ex.Message;
                return View("Register");
            }
        }

        [Route("ForgetPassword")]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [Route("NewPassword")]
        public IActionResult NewPassword()
        {
            return View();
        }
    }
}
