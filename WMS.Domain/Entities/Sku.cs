using System.ComponentModel.DataAnnotations.Schema;
using WMS.Domain.Common;

namespace WMS.Domain.Entities
{
	public class Sku : BaseEntity
	{
		public Guid ProductId { get; set; }
		public Guid UnitId { get; set; }
		public Guid LocationId { get; set; }
		public decimal Quantity { get; set; }


		[ForeignKey("ProductId")]
		public Product Product { get; set; }

		[ForeignKey("UnitId")]
		public Unit Unit { get; set; }

		[ForeignKey("LocationId")]
		public Location Location { get; set; }

	}
}
