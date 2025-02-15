using WMS.Application.Dtos.Sku;
using WMS.Domain.Entities;

namespace WMS.Application.Interfaces.Repositories
{
	public interface ISkuRepository : IRepository<Sku>
	{
		Task<Sku> AddASku(SkuInsertDto insertDto);
	}
}
