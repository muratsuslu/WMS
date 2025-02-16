using WMS.Domain.Enums;

namespace WMS.Application.Dtos
{
	public class OrderInsertDto
	{
		public Guid CustomerId { get; set; }
		public OrderPriority Priority { get; set; }
		public CompleteDeliveryRequired CompleteDeliveryRequired { get; set; }
		public OrderStatus OrderStatus { get; set; }
        public List<LineInsertDto> Lines { get; set; }
    }

	public class OrderUpdateDto
	{
		public Guid Id { get; set; }
		public Guid CustomerId { get; set; }
		public OrderPriority Priority { get; set; }
		public CompleteDeliveryRequired CompleteDeliveryRequired { get; set; }
		public OrderStatus OrderStatus { get; set; }
	}

	public class OrderDeleteDto
	{
		public Guid Id { get; set; }
	}
}
