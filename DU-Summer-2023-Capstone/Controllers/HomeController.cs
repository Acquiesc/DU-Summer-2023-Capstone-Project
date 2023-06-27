using Microsoft.AspNetCore.Mvc;

namespace DU_Summer_2023_Capstone.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            return View();
        }
    }
}
