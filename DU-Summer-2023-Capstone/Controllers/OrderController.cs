using DU_Summer_2023_Capstone.Data;
using DU_Summer_2023_Capstone.Data.Interfaces;
using DU_Summer_2023_Capstone.Data.Models;
using DU_Summer_2023_Capstone.PagesModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DU_Summer_2023_Capstone.Controllers
{
    public class OrderController : Controller
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly IOrderRepository _orderRepository;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly Cart _cart;
        private readonly ApplicationDbContext _context;


        public OrderController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IOrderRepository orderRepository, Cart cart)
        {
            _context = context;
            _orderRepository = orderRepository;
            _cart = cart;
            _userManager = userManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            var currentUser = _userManager.GetUserAsync(User).Result;
            var roles = _userManager.GetRolesAsync(currentUser).Result;

            if (roles.Contains("Admin"))
            {
                var orders = _orderRepository.Orders.ToList();
                return View(orders);
            }
            else
            {
                return RedirectToAction("OrderHistory");
            }


        }

        [Authorize(Roles = "Admin")]
        public IActionResult OrderView(int orderId)
        {
            // Retrieve order details based on the orderId
            var order = _orderRepository.Orders.FirstOrDefault(p => p.OrderId == orderId);
            ;
            if (order == null)
            {
                // Handle case where order is not found
                return RedirectToAction("Index", "Menu");
            }

            order.OrderLines = _orderRepository.GetOrderDetails(order);
            return View(order);

        }

        [Authorize]
        public IActionResult OrderHistory()
        {
            var currentUser = _userManager.GetUserAsync(User).Result;
            var orderHistory = new OrderHistoryModel
            {
                CompletedOrders = _orderRepository.GetCompleted(currentUser),
                PendingOrders = _orderRepository.GetPending(currentUser),
                CancelledOrders = _context.Orders.Where(o => o.Cancelled).ToList()
            };
            return View(orderHistory);
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult CompleteOrder(int orderId)
        {
            _orderRepository.MarkCompleted(orderId);
            return RedirectToAction("Index");
        }

        public IActionResult CancelOrder(int orderId)
        {
            _orderRepository.CancelOrder(orderId);
            return RedirectToAction("Index");
        }


        [Authorize]
        public IActionResult Checkout()
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
                return RedirectToAction("Index", "Cart");

            }

        }


        [HttpPost]
        [Authorize]
        public IActionResult Checkout(Order order)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;
            order.AspNetUserId = currentUser.Id;

            var items = _cart.GetCartItems();
            if (items.Count == 0)
            {
                ModelState.AddModelError("", "Your card is empty, add some drinks first");
            }


            _orderRepository.CreateOrder(order);
            _cart.ClearCart();
            return RedirectToAction("CheckoutComplete", new
            {
                order.OrderId
            });


        }
        [Authorize]
        public IActionResult CheckoutComplete(int orderId)
        {
            ViewBag.CheckoutCompleteMessage = "Thanks for your order! :) ";
            return RedirectToAction("OrderDetails", new
            {
                orderId
            });
        }

        [Authorize]
        public IActionResult OrderDetails(int orderId)
        {
            // Retrieve order details based on the orderId
            var order = _orderRepository.Orders.FirstOrDefault(p => p.OrderId == orderId);
            ;
            if (order == null)
            {
                // Handle case where order is not found
                return RedirectToAction("Index", "Menu");
            }

            order.OrderLines = _orderRepository.GetOrderDetails(order);


            var orderDetails = new OrderDetailsModel
            {
                Order = order
            };

            // Pass the order object to the view
            return View(orderDetails);
        }



    }
}
