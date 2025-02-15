using WMS.Domain.Common;
using WMS.Domain.Enums;

namespace WMS.Domain.Entities
{
	public class Order : BaseEntity
	{
		public Order()
		{
			Lines = new HashSet<Line>();
		}
		public OrderPriority Priority { get; set; }
		public CompleteDeliveryRequired CompleteDeliveryRequired { get; set; }
		public decimal Quantity { get; set; }
        public OrderStatus OrderStatus { get; set; }
		public virtual IEnumerable<Line> Lines { get; set; }
	}
}
