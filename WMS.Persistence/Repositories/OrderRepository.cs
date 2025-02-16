using AutoMapper;
using WMS.Application.Dtos;
using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;
using WMS.Persistence.Context;

namespace WMS.Persistence.Repositories
{
	public class OrderRepository : Repository<Order> , IOrderRepository
	{
		private readonly IMapper _mapper;
		public OrderRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
		{
			_mapper = mapper;
		}
	}
}
