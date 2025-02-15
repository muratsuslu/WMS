using WMS.Application.Dtos.Product;
using WMS.Domain.Entities;

namespace WMS.Application.Interfaces.Repositories
{
	public interface IProductRepository : IRepository<Product>
	{
		public Task<Product> AddAProduct(ProductInsertDto insert);
	}
}
