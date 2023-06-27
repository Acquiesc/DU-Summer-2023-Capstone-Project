using DU_Summer_2023_Capstone.Data.Interfaces;
using DU_Summer_2023_Capstone.Data.Models;
using DU_Summer_2023_Capstone.PagesModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DU_Summer_2023_Capstone.Controllers
{
    public class CartController : Controller
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly Cart _cart;

        public CartController(IPizzaRepository pizzaRepository, Cart cart)
        {
            _pizzaRepository = pizzaRepository;
            _cart = cart;
        }

        [Authorize]
        public ViewResult Index()
        {
            var items = _cart.GetCartItems();
            _cart.CartItems = items;

            var cartViewModel = new CartViewModel
            {
                Cart = _cart,
                CartTotal = _cart.GetCartTotal()
            };
            return View(cartViewModel);
        }

        [Authorize]
        public ViewResult Checkout()
        {
            var items = _cart.GetCartItems();
            var cartViewModel = new CartViewModel
            {
                Cart = _cart,
                CartTotal = _cart.GetCartTotal()
            };
            if (items != null && items.Count > 0)
            {
                return View(cartViewModel);
            }
            else
            {
                return View("Index", cartViewModel);

            }

        }

        public ViewResult OrderDetails()
        {
            return View();
        }

        [Authorize]
        public RedirectToActionResult AddToCart(int pizzaId)
        {
            var selectedPizza = _pizzaRepository.Pizzas.FirstOrDefault(p => p.PizzaId == pizzaId);
            if (selectedPizza != null)
            {
                _cart.AddToCart(selectedPizza, 1);
            }
            return RedirectToAction("Index", "Menu");
        }

        [Authorize]
        public RedirectToActionResult IncreaseAmmount(int pizzaId)
        {
            var selectedPizza = _pizzaRepository.Pizzas.FirstOrDefault(p => p.PizzaId == pizzaId);
            if (selectedPizza != null)
            {
                _cart.AddToCart(selectedPizza, 1);
            }
            return RedirectToAction("Index");
        }
        public RedirectToActionResult RemoveFromCart(int pizzaId)
        {
            Console.WriteLine(pizzaId);


            var selectedPizza = _pizzaRepository.Pizzas.FirstOrDefault(p => p.PizzaId == pizzaId);

            if (selectedPizza != null)
            {
                _cart.RemoveFromCart(selectedPizza);
            }
            return RedirectToAction("Index");
        }




    }
}
