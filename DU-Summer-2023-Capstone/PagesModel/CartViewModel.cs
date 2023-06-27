using DU_Summer_2023_Capstone.Data.Models;

namespace DU_Summer_2023_Capstone.PagesModel
{
    public class CartViewModel
    {
        public Cart Cart
        {
            get;
            set;
        }
        public decimal CartTotal
        {
            get;
            set;
        }

        public decimal GetTax()
        {
            decimal tax = 0.16m * CartTotal;
            return tax;
        }

        public decimal GetSubTotal()
        {
            return CartTotal + GetTax();
        }
    }
}
