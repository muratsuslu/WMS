using WMS.Application.Dtos.Product;
using WMS.Domain.Entities;

namespace WMS.Persistence.Mappers
{
	public static class ProductMapper
	{
		public static Product ConvertInsertDtoToProduct(ProductInsertDto insertDto)
		{
			return new Product()
			{
				Created = DateTime.Now,
				Deleted = null,
				Id = Guid.NewGuid(),
				IsDeleted = false,
				Name = insertDto.Name,
				Updated = null
			};
		}
	}
}
