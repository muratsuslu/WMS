using AutoMapper;
using WMS.Application.Dtos;
using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;
using WMS.Persistence.Context;

namespace WMS.Persistence.Repositories
{
	public class LineRepository : Repository<Line>, ILineRepository
	{
		private readonly IMapper _mapper;
		public LineRepository(ApplicationDbContext context, IMapper mapper) : base(context,mapper)
		{
			_mapper = mapper;
		}

	}
}
