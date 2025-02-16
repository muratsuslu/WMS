using AutoMapper;
using WMS.Application.Dtos;
using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;
using WMS.Persistence.Context;

namespace WMS.Persistence.Repositories
{
	public class CustomerRepository : Repository<Customer>, ICustomerRepository
	{
		private readonly IMapper _mapper;
		public CustomerRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
		{
			_mapper = mapper;
		}

	}
}
