using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Items> Items { get; set; }
        public DbSet<ItemGallery> ItemGallery { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set;}
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}

