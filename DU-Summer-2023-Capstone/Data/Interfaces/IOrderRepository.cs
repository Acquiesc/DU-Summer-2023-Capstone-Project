using DU_Summer_2023_Capstone.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace DU_Summer_2023_Capstone.Data.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> Orders
        {
            get;
        }

        IEnumerable<OrderDetail> OrderDetails
        {
            get;
        }

        List<OrderDetail> GetOrderDetails(Order order);

        List<Order> GetCompleted(IdentityUser user);

        List<Order> GetPending(IdentityUser user);

        void CreateOrder(Order order);
        void CancelOrder(int orderId);
        void MarkCompleted(int orderId);
    }
}
