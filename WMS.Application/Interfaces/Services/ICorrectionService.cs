using WMS.Application.Dtos;
using WMS.Domain.Entities;

namespace WMS.Application.Interfaces.Services
{
	public interface ICorrectionService
	{
		Task<ServiceReturnDto<Sku>> CorrectSku(Guid skuId, decimal change); 
	}
}
