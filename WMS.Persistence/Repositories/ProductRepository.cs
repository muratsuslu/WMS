using WMS.Application.Dtos.Product;
using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;
using WMS.Persistence.Context;
using WMS.Persistence.Mappers;

namespace WMS.Persistence.Repositories
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            
        }

		public async Task<Product> AddAProduct(ProductInsertDto insert)
		{
			try
			{
				Product productWillBeInserted = ProductMapper.ConvertInsertDtoToProduct(insert);
				productWillBeInserted = await AddAsync(productWillBeInserted);
				// TODO : Logging
				return productWillBeInserted;
			}
			catch (Exception ex)
			{
				// TODO : Logging
				return null;
			}

		}
	}
}
