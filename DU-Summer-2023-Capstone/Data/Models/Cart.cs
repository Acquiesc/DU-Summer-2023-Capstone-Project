using Microsoft.EntityFrameworkCore;

namespace DU_Summer_2023_Capstone.Data.Models
{
    public class Cart
    {
        private readonly ApplicationDbContext _appDbContext;
        private Cart(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public string CartId
        {
            get; set;
        }

        public List<CartItem> CartItems
        {
            get; set;
        }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext?.Session;

            var context = services.GetService<ApplicationDbContext>();
            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new Cart(context) { CartId = cartId };
        }

        public void AddToCart(Pizza pizza, int amount)
        {
            var cartItem =
                    _appDbContext.CartItems.SingleOrDefault(
                        s => s.Pizza.PizzaId == pizza.PizzaId && s.CartId == CartId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = CartId,
                    Pizza = pizza,
                    Amount = 1
                };

                _appDbContext.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Amount++;
            }
            _appDbContext.SaveChanges();
        }

        public int RemoveFromCart(Pizza pizza)
        {

            Console.WriteLine(pizza.Name);
            var cartItem =
                    _appDbContext.CartItems.SingleOrDefault(
                        s => s.Pizza.PizzaId == pizza.PizzaId && s.CartId == CartId);

            var localAmount = 0;

            if (cartItem != null)
            {
                if (cartItem.Amount > 1)
                {
                    cartItem.Amount--;
                    localAmount = cartItem.Amount;
                }
                else
                {
                    _appDbContext.CartItems.Remove(cartItem);
                }
            }

            _appDbContext.SaveChanges();

            return localAmount;
        }

        public List<CartItem> GetCartItems()
        {
            return CartItems ??
                   (CartItems =
                       _appDbContext.CartItems.Where(c => c.CartId == CartId)
                           .Include(s => s.Pizza)
                           .ToList());
        }

        public void ClearCart()
        {
            var cartItems = _appDbContext
                .CartItems
                .Where(cart => cart.CartId == CartId);

            _appDbContext.CartItems.RemoveRange(cartItems);

            _appDbContext.SaveChanges();
        }

        public decimal GetCartTotal()
        {
            var total = _appDbContext.CartItems.Where(c => c.CartId == CartId)
                .Select(c => c.Pizza.Price * c.Amount).Sum();
            return total;
        }
    }
}
