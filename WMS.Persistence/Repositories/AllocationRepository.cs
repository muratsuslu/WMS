using AutoMapper;
using WMS.Application.Dtos;
using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;
using WMS.Persistence.Context;

namespace WMS.Persistence.Repositories
{
	public class AllocationRepository : Repository<Allocation> , IAllocationRepository
	{
		private readonly IMapper _mapper;
		public AllocationRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
			_mapper = mapper;
		}

	}
}
