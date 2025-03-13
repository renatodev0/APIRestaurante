using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using APIRestaurante.Domain.Entities;

namespace APIRestaurante.Infrastructure.Data
{
    public class RestaurantContext : IdentityDbContext<User>
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> options)
            : base(options)
        { }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
