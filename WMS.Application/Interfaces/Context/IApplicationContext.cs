using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;

namespace WMS.Application.Interfaces.Context
{
    public interface IApplicationContext
    {
        public DbSet<Line> Lines { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderSku> OrderSkus { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sku> Skus { get; set; }
    }
}
