using APS_01_2021.Models;
using APS_01_2021.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APS_01_2021.Controllers
{
    public class ContactController : Controller
    {
        private UserService _userService;
        private ContactService _contactService;

        public ContactController(UserService userService, ContactService contactService)
        {
            _userService = userService;
            _contactService = contactService;
        }


        public async Task<List<ContactModel>> GetContactList()
        {
            var userNickname = User.Claims.First().Value;
            var list = await _contactService.FindAllByNickName(userNickname);
            return list;
        }
    }
}
