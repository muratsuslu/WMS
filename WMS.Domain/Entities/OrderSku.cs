using System.ComponentModel.DataAnnotations.Schema;
using WMS.Domain.Common;
using WMS.Domain.Enums;

namespace WMS.Domain.Entities
{
	public class OrderSku : BaseEntity
	{
        public Guid OrderId { get; set; }
        public Guid SkuId { get; set; }
        public OrderSkuStatus OrderSkuStatus { get; set; }

		[ForeignKey("OrderId")]
		public Order Order { get; set; }

		[ForeignKey("SkuId")]
		public Sku Sku { get; set; }
	}
}
