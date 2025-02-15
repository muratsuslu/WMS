using WMS.Application.Dtos.Unit;
using WMS.Domain.Entities;

namespace WMS.Persistence.Mappers
{
	public static class UnitMapper
	{
		public static Unit ConvertInsertDtoToUnit(UnitInsertDto insertDto)
		{
			return new Unit()
			{
				Created = DateTime.Now,
				Deleted = null,
				Id = Guid.NewGuid(),
				IsDeleted = false,
				Name = insertDto.Name,
				Updated = null
			};
		}
	}
}
