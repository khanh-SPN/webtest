using Microsoft.EntityFrameworkCore;
using webquanlydouong.Models;

namespace webquanlydouong.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithMany(p => p.Orders);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@example.com",
                    Password = "123",  // Lưu mật khẩu không mã hóa (không an toàn)
                    EmailVerified = true,
                    Role = "Admin"
                },
                new User
                {
                    Id = 2,
                    Username = "user",
                    Email = "user@example.com",
                    Password = "123",  // Lưu mật khẩu không mã hóa (không an toàn)
                    EmailVerified = true,
                    Role = "User"
                }
            );
        }
    }
}
