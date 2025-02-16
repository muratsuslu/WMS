using System.ComponentModel.DataAnnotations.Schema;
using WMS.Domain.Common;
using WMS.Domain.Enums;

namespace WMS.Domain.Entities
{
	public class Sku : BaseEntity
	{
		public Guid ProductId { get; set; }
		public Guid LocationId { get; set; }
		public decimal Quantity { get; set; }
        public SkuStatus SkuStatus { get; set; }

        [ForeignKey("ProductId")]
		public Product Product { get; set; }

		[ForeignKey("LocationId")]
		public Location Location { get; set; }

	}
}
