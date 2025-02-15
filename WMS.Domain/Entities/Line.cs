using System.ComponentModel.DataAnnotations.Schema;
using WMS.Domain.Common;
using WMS.Domain.Enums;

namespace WMS.Domain.Entities
{
	public class Line : BaseEntity
	{

        public Guid ProductId { get; set; }
		public Guid OrderId { get; set; }
		public decimal Quantity { get; set; }
        public LineStatus LineStatus { get; set; }

		[ForeignKey("ProductId")]
		public  Product Product { get; set; }

		[ForeignKey("OrderId")]
		public Order Order { get; set; }
	}
}
