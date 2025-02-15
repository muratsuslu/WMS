using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;
using WMS.Persistence.Context;

namespace WMS.Persistence.Repositories
{
	public class SkuRepository : Repository<Sku>, ISkuRepository
	{
        public SkuRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
