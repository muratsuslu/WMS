using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;
using WMS.Persistence.Context;

namespace WMS.Persistence.Repositories
{
	public class LineRepository : Repository<Line> , ILineRepository
	{
		public LineRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
