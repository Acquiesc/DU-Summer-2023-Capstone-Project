using DU_Summer_2023_Capstone.Data.Interfaces;
using DU_Summer_2023_Capstone.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DU_Summer_2023_Capstone.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly Cart _shoppingCart;


        public OrderRepository(ApplicationDbContext appDbContext, Cart shoppingCart)
        {
            _appDbContext = appDbContext;
            _shoppingCart = shoppingCart;
        }

        public IEnumerable<Order> Orders => _appDbContext.Orders;

        public IEnumerable<OrderDetail> OrderDetails => _appDbContext.OrderDetails;

        public void CancelOrder(int orderId)
        {
            var newOrder = _appDbContext.Orders.Find(orderId);
            newOrder.Completed = false;
            newOrder.Cancelled = true;

            _appDbContext.SaveChanges();

        }

        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;
            decimal tax = 0.16m * _shoppingCart.GetCartTotal();
            order.OrderTotal = _shoppingCart.GetCartTotal() + tax;

            _appDbContext.Orders.Add(order);

            var shoppingCartItems = _shoppingCart.CartItems;



            _appDbContext.SaveChanges();

            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Amount = shoppingCartItem.Amount,
                    PizzaId = shoppingCartItem.Pizza.PizzaId,
                    OrderId = order.OrderId,
                    Price = shoppingCartItem.Pizza.Price
                };

                _appDbContext.OrderDetails.Add(orderDetail);
            }

            _appDbContext.SaveChanges();


        }

        public List<Order> GetCompleted(IdentityUser user)
        {
            var orders = new List<Order>();


            foreach (var order in _appDbContext.Orders)
            {
                var tempOrder = order;
                tempOrder.OrderLines = GetOrderDetails(order);
                if (tempOrder.AspNetUserId == user.Id && (tempOrder.CalculateTimeRemaining() < 1 || tempOrder.Completed) && !tempOrder.Cancelled)
                {
                    orders.Add(tempOrder);

                }
            }

            return orders;
        }

        public List<OrderDetail> GetOrderDetails(Order order)
        {
            var orderDetalis = new List<OrderDetail>();

            foreach (var item in OrderDetails)
            {
                var orderDetail = _appDbContext.OrderDetails
                                .Include(od => od.Pizza)
                                .FirstOrDefault(od => od.OrderDetailId == item.OrderDetailId);
                if (orderDetail.OrderId == order.OrderId)
                {
                    orderDetalis.Add(orderDetail);
                }
            }

            return orderDetalis;

        }

        public List<Order> GetPending(IdentityUser user)
        {
            var orders = new List<Order>();


            foreach (var order in _appDbContext.Orders)
            {
                var tempOrder = order;
                tempOrder.OrderLines = GetOrderDetails(order);
                if (tempOrder.AspNetUserId == user.Id && tempOrder.CalculateTimeRemaining() > 1 && (!tempOrder.Cancelled))
                {
                    orders.Add(tempOrder);

                }
            }

            return orders;
        }

        public void MarkCompleted(int orderId)
        {
            var newOrder = _appDbContext.Orders.Find(orderId);
            newOrder.Completed = true;
            newOrder.Cancelled = false;

            _appDbContext.SaveChanges();
        }
    }
}
