using Lesson_22_Pizza_Star.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pizza_Star.Models;
using Pizza_Star.Models.Checkout;

namespace Lesson_22_Pizza_Star.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Rating> Ratings { get; set; } = null!;
        public DbSet<ShopCartItem> ShopCartItems { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderDetails> OrderDetails { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShopCartItem>()
                .Property(u=> u.CreatedAt)
                .HasDefaultValueSql("GETDATE()");


            base.OnModelCreating(modelBuilder);
        }
    }
}
