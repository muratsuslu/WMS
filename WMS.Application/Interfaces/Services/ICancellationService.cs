using WMS.Application.Dtos;
using WMS.Domain.Entities;

namespace WMS.Application.Interfaces.Services
{
	public interface ICancellationService
	{
		Task<ServiceReturnDto<Order>> CancelAnOrder(Guid orderId);
		Task<ServiceReturnDto<Line>> CancelASingleLine(Guid lineId);
	}
}
