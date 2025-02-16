using AutoMapper;
using WMS.Application.Dtos;
using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;
using WMS.Persistence.Context;

namespace WMS.Persistence.Repositories
{
	public class SkuRepository : Repository<Sku>, ISkuRepository
	{
		private readonly IMapper _mapper;
		public SkuRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
			_mapper = mapper;
		}
	}
}
