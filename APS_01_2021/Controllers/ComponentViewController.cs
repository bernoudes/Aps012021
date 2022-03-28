using Microsoft.AspNetCore.Mvc;

namespace APS_01_2021.Controllers
{
    [Route("cvc")]
    public class ComponentViewController : Controller
    {
        //-------------------------------------------
        //put here all the viewcomponents who need to be call by front directly
        public ActionResult CallInviteAccept()
        {
            return ViewComponent("InviteAccept");
        }
    }
}
