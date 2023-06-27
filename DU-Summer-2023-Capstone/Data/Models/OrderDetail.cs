namespace DU_Summer_2023_Capstone.Data.Models
{
    public class OrderDetail
    {
        public int OrderDetailId
        {
            get; set;
        }
        public int OrderId
        {
            get; set;
        }
        public int PizzaId
        {
            get; set;
        }
        public int Amount
        {
            get; set;
        }
        public decimal Price
        {
            get; set;
        }
        public virtual Pizza Pizza
        {
            get; set;
        }
        public virtual Order Order
        {
            get; set;
        }

        public decimal GetTotal()
        {
            return Amount * Pizza.Price;
        }
    }
}
