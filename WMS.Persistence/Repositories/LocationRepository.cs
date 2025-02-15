using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;
using WMS.Persistence.Context;

namespace WMS.Persistence.Repositories
{
	public class LocationRepository : Repository<Location> , ILocationRepository
	{
        public LocationRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
