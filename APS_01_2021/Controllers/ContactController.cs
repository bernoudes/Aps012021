using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APS_01_2021.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return ViewComponent("InviteContactCreate");
        }
    }
}
