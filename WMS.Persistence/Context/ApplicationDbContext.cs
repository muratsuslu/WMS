using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces.Context;
using WMS.Domain.Entities;
namespace WMS.Persistence.Context
{
	public class ApplicationDbContext : DbContext, IApplicationContext
	{
		public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
		{ }
		public DbSet<Line> Lines {get;set;}
		public DbSet<Location> Locations {get;set;}
		public DbSet<Order> Orders {get;set;}
		public DbSet<Allocation> Allocations {get;set;}
		public DbSet<Product> Products {get;set;}
		public DbSet<Sku> Skus {get;set;}
        public DbSet<Customer> Customers { get; set; }
    }
}
