using App.Services.Exceptions;
using APS_01_2021.Models;
using APS_01_2021.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Chat", "MainPage");
            }
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserModel user)
        {
            var confirmUser = await _userService.FindByNickName(user.NickName);
            if (confirmUser != null && confirmUser.IsConfirmPasswordConfirmed(confirmUser.Password, user.Password))
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("usernickname", user.NickName));
                claims.Add(new Claim(ClaimTypes.NameIdentifier , user.NickName));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return RedirectToAction("Chat","MainPage");
            }
            else
            {
                TempData["Error"] = "Usuario ou senha errados";
                return View("Login");
            }
        }
        //REGISTER AREA
        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Chat", "MainPage");
            }
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

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [Route("ForgetPassword")]
        public IActionResult ForgetPassword()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Chat", "MainPage");
            }
            return View();
        }

        [Route("NewPassword")]
        public IActionResult NewPassword()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Chat", "MainPage");
            }
            return View();
        }
    }
}
