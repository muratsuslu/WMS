using WMS.Domain.Enums;

namespace WMS.Application.Dtos
{
	public class LineInsertDto
	{
		public Guid ProductId { get; set; }
		public decimal Quantity { get; set; }
		public LineStatus LineStatus { get; set; }
	}

	public class LineUpdateDto
	{
		public Guid Id { get; set; }
		public Guid ProductId { get; set; }
		public Guid OrderId { get; set; }
		public decimal Quantity { get; set; }
		public LineStatus LineStatus { get; set; }
	}

	public class LineDeleteDto
	{
		public Guid Id { get; set; }
	}
}
