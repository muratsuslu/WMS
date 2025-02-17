using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WMS.Domain.Common;
using WMS.Domain.Enums;

namespace WMS.Domain.Entities
{
	public class Line : BaseEntity
	{

        public Guid ProductId { get; set; }
		public Guid OrderId { get; set; }
        public Guid? SkuId { get; set; }
        public decimal Quantity { get; set; }
        public LineStatus LineStatus { get; set; }

		[ForeignKey("SkuId")]
		public Sku Sku { get; set; }

		[ForeignKey("ProductId")]
		public  Product Product { get; set; }

		[ForeignKey("OrderId")]
		[JsonIgnore]
		public Order Order { get; set; }
	}
}
