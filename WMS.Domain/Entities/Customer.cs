using System.Text.Json.Serialization;
using WMS.Domain.Common;

namespace WMS.Domain.Entities
{
	public class Customer : BaseEntity
	{
        public Customer()
        {
            Orders = new HashSet<Order>();
        }
        public string FullName { get; set; }
		public string Email { get; set; }
        [JsonIgnore]
        public virtual IEnumerable<Order> Orders { get; set; }
    }
}
