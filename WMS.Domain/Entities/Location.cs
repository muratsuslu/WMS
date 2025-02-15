using WMS.Domain.Common;
using WMS.Domain.Enums;

namespace WMS.Domain.Entities
{
	public class Location : BaseEntity
	{
        public string Name { get; set; }
        public IsLocked IsLocked { get; set; }

    }
}
