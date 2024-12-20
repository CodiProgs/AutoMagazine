using Microsoft.EntityFrameworkCore;

namespace AutoMagazine.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<Models.Entities.Cart> Carts { get; set; }
        public DbSet<Models.Entities.CartItem> CartItems { get; set; }
        public DbSet<Models.Entities.Category> Categories { get; set; }
        public DbSet<Models.Entities.Order> Orders { get; set; }
        public DbSet<Models.Entities.OrderItem> OrderItems { get; set; }
        public DbSet<Models.Entities.Product> Products { get; set; }
        public DbSet<Models.Entities.User> Users { get; set; }
    }
}
