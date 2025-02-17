
using WMS.Application.Dtos;
using WMS.Domain.Entities;

namespace WMS.Application.Interfaces.Services
{
	public interface IAllocationService 
	{
		Task<ServiceReturnDto<IEnumerable<Allocation>>> AllocateAnOrder(Guid orderId);
		Task<ServiceReturnDto<IEnumerable<Allocation>>> AllocateAllOrders();
	}

}
