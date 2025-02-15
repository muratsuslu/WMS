using WMS.Application.Dtos.Unit;
using WMS.Domain.Entities;

namespace WMS.Application.Interfaces.Repositories
{
	public interface IUnitRepository : IRepository<Unit>
	{
		Task<Unit> AddAUnit(UnitInsertDto unit);
	}
}
