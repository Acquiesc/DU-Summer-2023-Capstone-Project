using DU_Summer_2023_Capstone.Data.Models;
using DU_Summer_2023_Capstone.PagesModel;
using Microsoft.AspNetCore.Mvc;

namespace DU_Summer_2023_Capstone.Components
{
    public class CartSummary : ViewComponent
    {
        private readonly Cart _cart;
        public CartSummary(Cart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
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

    }
}
