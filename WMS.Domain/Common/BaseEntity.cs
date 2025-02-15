using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Common
{
	public class BaseEntity
	{
        [Key]
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? Deleted { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
