using DU_Summer_2023_Capstone.Data.Models;

namespace DU_Summer_2023_Capstone.PagesModel
{
    public class OrderHistoryModel
    {
        public List<Order> CompletedOrders
        {
            get;
            set;
        }

        public List<Order> PendingOrders
        {
            get; set;
        }


        public List<Order> CancelledOrders
        {
            get; set;
        }

    }
}
