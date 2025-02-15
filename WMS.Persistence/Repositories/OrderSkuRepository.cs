using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;
using WMS.Persistence.Context;

namespace WMS.Persistence.Repositories
{
	public class OrderSkuRepository : Repository<OrderSku> , IOrderSkuRepository
	{
        public OrderSkuRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
