using Microsoft.AspNetCore.Mvc;

namespace DU_Summer_2023_Capstone.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult OrderHistory()
        {
            return View("~/Pages/Profile/OrderHistory.cshtml");
        }
    }
}
