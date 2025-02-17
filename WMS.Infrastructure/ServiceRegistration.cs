using Microsoft.Extensions.DependencyInjection;
using WMS.Application.Interfaces.Repositories;
using WMS.Application.Interfaces.Services;
using WMS.Infrastructure.Services;

namespace WMS.Infrastructure
{
	public static class ServiceRegistration
	{
		public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<IAllocationService, AllocationService>();
			serviceCollection.AddScoped<ICancellationService, CancellationService>();
			serviceCollection.AddScoped<ICorrectionService, CorrectionService>();
		}
	}
}
