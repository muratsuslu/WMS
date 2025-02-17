using WMS.Domain.Enums;

namespace WMS.Application.Dtos
{
	public class AllocationInsertDto
	{
		public Guid SkuId { get; set; }
		public Guid LineId { get; set; }
		public AllocationStatus AllocationStatus { get; set; }
	}

	public class AllocationUpdateDto
	{
		public Guid Id { get; set; }
		public Guid LineId { get; set; }
		public Guid SkuId { get; set; }
		public AllocationStatus AllocationStatus { get; set; }
	}

	public class AllocationDeleteDto
	{
		public Guid Id { get; set; }
	}
}
