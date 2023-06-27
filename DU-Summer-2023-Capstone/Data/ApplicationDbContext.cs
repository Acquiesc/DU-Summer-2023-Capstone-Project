using DU_Summer_2023_Capstone.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DU_Summer_2023_Capstone.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pizza>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");

            // Other configurations...

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Pizza> Pizzas
        {
            get; set;
        }

        public DbSet<CartItem> CartItems
        {
            get; set;
        }

        public DbSet<Order> Orders
        {
            get; set;
        }
        public DbSet<OrderDetail> OrderDetails
        {
            get; set;
        }
    }
}