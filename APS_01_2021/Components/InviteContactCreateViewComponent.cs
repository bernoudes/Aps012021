﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APS_01_2021.Components
{
    public class InviteContactCreateViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
