using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Dtos;
using WMS.Application.Interfaces.Services;
using WMS.Application.Wrappers;
using WMS.Domain.Entities;

namespace WMS.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AllocationController : ControllerBase
	{
		protected readonly IAllocationService _allocationService;
		public AllocationController(IAllocationService allocationService)
        {
            _allocationService = allocationService;
        }


		[HttpPost("allocate-an-order")]
		public async Task<IActionResult> AllocateAnOrder(Guid orderId)
		{
			var result = await _allocationService.AllocateAnOrder(orderId);
			if (result.IsSuccess)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result.Message);
			}	
		}

		[HttpPost("allocate-all-orders")]
		public async Task<IActionResult> AllocateAllOrders()
		{
			var result = await _allocationService.AllocateAllOrders();
			if (result.IsSuccess)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result.Message);
			}
		}


	}
}
