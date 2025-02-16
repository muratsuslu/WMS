using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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
        public Guid CustomerId { get; set; }
        public OrderPriority Priority { get; set; }
		public CompleteDeliveryRequired CompleteDeliveryRequired { get; set; }
        public OrderStatus OrderStatus { get; set; }
		public virtual IEnumerable<Line> Lines { get; set; }
		[ForeignKey("CustomerId")]
		[JsonIgnore]
		public Customer Customer { get; set; }
    }
}
