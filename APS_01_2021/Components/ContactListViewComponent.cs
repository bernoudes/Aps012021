using APS_01_2021.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APS_01_2021.Components
{
    public class ContactListViewComponent : ViewComponent
    {

        private ContactService _contactService;
        public ContactListViewComponent(ContactService contactService)
        {
            _contactService = contactService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {


            var userNickname = UserClaimsPrincipal.Claims.First().Value;
            var list = await _contactService.FindAllByNickName(userNickname);
           
            return View(list);
        }
    }
}
