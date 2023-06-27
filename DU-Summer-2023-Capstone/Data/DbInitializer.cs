using DU_Summer_2023_Capstone.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DU_Summer_2023_Capstone.Data
{
    public class DbInitializer
    {
        public static void Seed(WebApplication applicationBuilder)
        {
            var scope = applicationBuilder.Services.CreateScope();
            ApplicationDbContext context =
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();


            // Find orders with total = 0
            var ordersToUpdate = context.Orders.Where(o => o.OrderTotal == 0).ToList();
            foreach (var existingOrder in ordersToUpdate)
            {
                var orderDetails = context.OrderDetails
                    .Where(od => od.OrderId == existingOrder.OrderId)
                    .Include(od => od.Pizza)
                    .ToList();

                decimal orderTotal = orderDetails.Sum(od => od.Amount * od.Pizza.Price);
                existingOrder.OrderTotal = orderTotal + (0.16m * orderTotal);
            }



            context.SaveChanges();


            var unknownOrders = context.Orders.Where(o => o.Cancelled == false || o.Completed == false).ToList();

            foreach (var unknownOrder in unknownOrders)
            {
                var orderDetails = context.OrderDetails
                    .Where(od => od.OrderId == unknownOrder.OrderId)
                    .Include(od => od.Pizza)
                    .ToList();

                unknownOrder.OrderLines = orderDetails;
                if (unknownOrder.CalculateTimeRemaining() <= 0 && !unknownOrder.Cancelled)
                {
                    unknownOrder.Completed = true;
                }

            }

            context.SaveChanges();

            if (!context.Pizzas.Any())
            {
                //var pizzas = new List<Pizza>
                //{
                //    new Pizza
                //    {
                //        Name = "Alfredo Pizza",
                //        ShortDescription = "Our signature crust topped with fresh tomato and creamy alfredo sauces",
                //        LongDescription = "",
                //        Price = 10.00m,
                //        ImageUrl = "/images/alfredo.jpeg",
                //        ImageThumbnailUrl = "/images/alfredo.jpeg",
                //        InStock = true
                //    },
                //    new Pizza
                //    {
                //        Name = "Buffalo Chicken Pizza",
                //        ShortDescription = "Our signature crust topped with stringy mozzarella cheese & chunks of our signature barbecue chicken",
                //        LongDescription = "",
                //        Price = 10.00m,
                //        ImageUrl = "/images/bbq-chicken.jpg",
                //        ImageThumbnailUrl = "/images/bbq-chicken.jpg",
                //        InStock = true
                //    },
                //    new Pizza
                //    {
                //        Name = "Buffalo Chicken Pizza",
                //        ShortDescription = "Our signature crust topped with stringy mozzarella cheese & chunks of our signature buffalo chicken",
                //        LongDescription = "",
                //        Price = 10.00m,
                //        ImageUrl = "/images/buffalo-chicken.jpeg",
                //        ImageThumbnailUrl = "/images/buffalo-chicken.jpeg",
                //        InStock = true
                //    },
                //    // Add more pizza objects as needed, following the same pattern
                //};

                var pizzas = new List<Pizza>
                {

                    new Pizza
                    {
                        Name = "Barbecue Chicken Pizza",
                        ShortDescription = "Our signature crust topped with stringy mozzarella cheese & chunks of our signature barbecue chicken",
                        LongDescription = "",
                        Price = 10.00m,
                        ImageUrl = "/images/bbq-chicken.jpg",
                        ImageThumbnailUrl = "/images/bbq-chicken.jpg",
                        InStock = true
                    },
                    new Pizza
                    {
                        Name = "Cheese Pizza",
                        ShortDescription = "Our signature crust topped with creamy tomato sauce and stringy mozzarella cheese",
                        LongDescription = "",
                        Price = 10.00m,
                        ImageUrl = "/images/cheese.jpeg",
                        ImageThumbnailUrl = "/images/cheese.jpeg",
                        InStock = true
                    },
                    new Pizza
                    {
                        Name = "Meat Mayhem Pizza",
                        ShortDescription = "Our signature crust topped with stringy mozzarella cheese along with sausage, pepperoni, bacon, ham, and chicken",
                        LongDescription = "",
                        Price = 10.00m,
                        ImageUrl = "/images/meat-mayhem.jpeg",
                        ImageThumbnailUrl = "/images/meat-mayhem.jpeg",
                        InStock = true
                    },
                    new Pizza
                    {
                        Name = "Pepperoni Pizza",
                        ShortDescription = "Our signature crust topped with stringy mozzarella cheese & pepperoni",
                        LongDescription = "",
                        Price = 10.00m,
                        ImageUrl = "/images/pepperoni.jpeg",
                        ImageThumbnailUrl = "/images/pepperoni.jpeg",
                        InStock = true
                    },
                    new Pizza
                    {
                        Name = "Sausage Pizza",
                        ShortDescription = "Our signature crust topped with stringy mozzarella cheese & sausage",
                        LongDescription = "",
                        Price = 10.00m,
                        ImageUrl = "/images/sausage.jpeg",
                        ImageThumbnailUrl = "/images/sausage.jpeg",
                        InStock = true
                    },
                    new Pizza
                    {
                        Name = "Supreme Pizza",
                        ShortDescription = "Our signature crust topped with stringy mozzarella cheese along with green peppers, red onions, mushrooms, pepperoni, and sausage",
                        LongDescription = "",
                        Price = 10.00m,
                        ImageUrl = "/images/supreme.jpeg",
                        ImageThumbnailUrl = "/images/supreme.jpeg",
                        InStock = true
                    },
                    new Pizza
                    {
                        Name = "Taco Pizza",
                        ShortDescription = "Our signature crust topped with stringy mozzarella cheese along with burger, lettuce, and tomato",
                        LongDescription = "",
                        Price = 10.00m,
                        ImageUrl = "/images/taco.webp",
                        ImageThumbnailUrl = "/images/taco.webp",
                        InStock = true
                    },
                    new Pizza
                    {
                        Name = "Veggie Pizza",
                        ShortDescription = "Our signature crust topped with stringy mozzarella cheese along with mushrooms, green peppers, and onions",
                        LongDescription = "",
                        Price = 10.00m,
                        ImageUrl = "/images/veggie.jpeg",
                        ImageThumbnailUrl = "/images/veggie.jpeg",
                        InStock = true
                    }
                };



                context.Pizzas.AddRange(pizzas);
                context.SaveChanges();
            }




        }


    }
}
