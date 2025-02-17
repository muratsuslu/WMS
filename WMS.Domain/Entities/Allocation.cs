using System.ComponentModel.DataAnnotations.Schema;
using WMS.Domain.Common;
using WMS.Domain.Enums;

namespace WMS.Domain.Entities
{
	public class Allocation : BaseEntity
	{
        public Guid LineId { get; set; }
        public Guid SkuId { get; set; }
        public AllocationStatus AllocationStatus { get; set; }

		[ForeignKey("LineId")]
		public  Line Line {get; set; }

		[ForeignKey("SkuId")]
		public Sku Sku { get; set; }
	}
}
