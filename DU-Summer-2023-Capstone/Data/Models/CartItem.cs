namespace DU_Summer_2023_Capstone.Data.Models
{
    public class CartItem
    {
        public int CartItemId
        {
            get; set;
        }
        public Pizza Pizza
        {
            get; set;
        }
        public int Amount
        {
            get; set;
        }
        public string CartId
        {
            get; set;
        }

        public decimal GetSubTotal()
        {
            return Amount * Pizza.Price;
        }

    }
}
