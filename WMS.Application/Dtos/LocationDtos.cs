using WMS.Domain.Enums;

namespace WMS.Application.Dtos
{
	public class LocationInsertDto
	{
		public string Name { get; set; }
		public IsLocked IsLocked { get; set; }
	}

	public class LocationUpdateDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public IsLocked IsLocked { get; set; }
	}

	public class LocationDeleteDto
	{
		public Guid Id { get; set; }
	}
}
