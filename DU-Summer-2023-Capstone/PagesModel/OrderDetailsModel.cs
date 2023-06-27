using DU_Summer_2023_Capstone.Data.Models;

namespace DU_Summer_2023_Capstone.PagesModel
{
    public class OrderDetailsModel
    {
        public Order Order
        {
            get; set;
        }

        public decimal GetTax()
        {
            decimal tax = 0.16m * Order.GetTotal();
            return tax;
        }

        public decimal GetSubTotal()
        {
            return Order.GetTotal() + GetTax();
        }
    }
}
