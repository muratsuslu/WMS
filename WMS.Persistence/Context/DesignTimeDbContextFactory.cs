using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace WMS.Persistence.Context
{
	public abstract class DesignTimeDbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
	{
		protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);
		public TContext CreateDbContext(string[] args)
		{
			DbContextOptionsBuilder<TContext> builder = new DbContextOptionsBuilder<TContext>();

			// Çalıştırılabilir dosyanın olduğu yerden 2 klasör yukarı çık (Solution seviyesine ulaş)
			string solutionDirectory = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.FullName;

			// Presentation/WebAPI klasörüne gir
			string webApiPath = Path.Combine(solutionDirectory, "MVC.WebAPI");

			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(webApiPath)  // WebAPI projesinin kök dizinini kullan
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			builder.UseSqlServer(configuration.GetConnectionString("SQLConnection"));

			return CreateNewInstance(builder.Options);
		}
	}
}
