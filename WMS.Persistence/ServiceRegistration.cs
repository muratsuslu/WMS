using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WMS.Application.Interfaces.Repositories;
using WMS.Persistence.Context;
using WMS.Persistence.Repositories;

namespace WMS.Persistence
{
	public static class ServiceRegistration
	{
		public static void AddPersistenceServices(this IServiceCollection serviceCollection, IConfiguration configuration = null)
		{
			serviceCollection.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(configuration?.GetConnectionString("SQLConnection")));
			serviceCollection.AddScoped<ILineRepository, LineRepository>();
			serviceCollection.AddScoped<ILocationRepository, LocationRepository>();
			serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
			serviceCollection.AddScoped<IOrderSkuRepository, OrderSkuRepository>();
			serviceCollection.AddScoped<IProductRepository, ProductRepository>();
			serviceCollection.AddScoped<ISkuRepository, SkuRepository>();
			serviceCollection.AddScoped<IUnitRepository, UnitRepository>();
		}
	}
}
