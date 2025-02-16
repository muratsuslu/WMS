using WMS.Domain.Enums;

namespace WMS.Application.Dtos
{
	public class SkuInsertDto
	{
		public Guid ProductId { get; set; }
		public Guid LocationId { get; set; }
		public decimal Quantity { get; set; }
		public SkuStatus SkuStatus { get; set; }
	}

	public class SkuUpdateDto
	{
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public Guid LocationId { get; set; }
		public decimal Quantity { get; set; }
		public SkuStatus SkuStatus { get; set; }
	}

	public class SkuDeleteDto
	{
		public Guid Id { get; set; }
	}
}
