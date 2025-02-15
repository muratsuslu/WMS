using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;
using WMS.Persistence.Context;

namespace WMS.Persistence.Repositories
{
	public class OrderRepository : Repository<Order> , IOrderRepository
	{
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
