using System.Collections.Generic;
using WMS.Application.Dtos.Unit;
using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;
using WMS.Persistence.Context;
using WMS.Persistence.Mappers;

namespace WMS.Persistence.Repositories
{
	internal class UnitRepository : Repository<Unit>, IUnitRepository
	{
        public UnitRepository(ApplicationDbContext context) : base(context)
        {
            
        }

		public async Task<Unit> AddAUnit(UnitInsertDto insert)
		{
			try
			{
				Unit unitWillBeInserted = UnitMapper.ConvertInsertDtoToUnit(insert);
				unitWillBeInserted = await AddAsync(unitWillBeInserted);
				// TODO : Logging
				return unitWillBeInserted;
			}
			catch (Exception ex)
			{
				// TODO : Logging
				return null;
			}

		}
	}
}
