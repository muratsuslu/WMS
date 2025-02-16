using WMS.Domain.Enums;

namespace WMS.Application.Dtos
{
	public class OrderSkuInsertDto
	{
		public Guid OrderId { get; set; }
		public Guid SkuId { get; set; }
		public OrderSkuStatus OrderSkuStatus { get; set; }
	}

	public class OrderSkuUpdateDto
	{
		public Guid Id { get; set; }
		public Guid OrderId { get; set; }
		public Guid SkuId { get; set; }
		public OrderSkuStatus OrderSkuStatus { get; set; }
	}

	public class OrderSkuDeleteDto
	{
		public Guid Id { get; set; }
	}
}
