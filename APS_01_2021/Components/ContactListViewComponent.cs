using APS_01_2021.Services;
using APS_01_2021.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APS_01_2021.Components
{
    public class ContactListViewComponent : ViewComponent
    {

        private readonly ContactService _contactService;
        private readonly UserService _userService;

        public ContactListViewComponent(ContactService contactService, UserService userService)
        {
            _contactService = contactService;
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {


            var userNickname = UserClaimsPrincipal.Claims.First().Value;
            var userid = await _userService.FindIdByNickName(userNickname);
            var list = await _contactService.FindAllByNickNameAsync(userNickname);

            List<ContactListViewModel> listView = new ();

            foreach (var item in list) 
            {
                listView.Add(new ContactListViewModel(item, userid));
            }

            return View(listView);
        }
    }
}
