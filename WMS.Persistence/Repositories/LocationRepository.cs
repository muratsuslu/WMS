using AutoMapper;
using WMS.Application.Dtos;
using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;
using WMS.Persistence.Context;

namespace WMS.Persistence.Repositories
{
	public class LocationRepository : Repository<Location>, ILocationRepository
	{
		private readonly IMapper _mapper;
		public LocationRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
		{
			_mapper = mapper;
		}
	}
}
