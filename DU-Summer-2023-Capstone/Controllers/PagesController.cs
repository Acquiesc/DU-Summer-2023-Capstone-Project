using Microsoft.AspNetCore.Mvc;

namespace DU_Summer_2023_Capstone.Controllers
{
    public class PagesController : Controller
    {
        //Use controller methods to access & deliver data before the page loads to display on page
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Menu()
        {
            //ie: can search for current user & their most ordered products, then return all products ordered by most purchased
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }


        /*
        public IActionResult Checkout()
        {
            return View();
        }

        
        public IActionResult storeOrder()
        {
            //validate request
                //if not validated -> return back with errors

            //if validated
            
            //store the data

            //return order success page
        }
        */
    }
}