using Microsoft.AspNetCore.Mvc;
using WMS.Application.Dtos;
using WMS.Application.Interfaces.Repositories;
using WMS.Application.Wrappers;
using WMS.Domain.Entities;
using WMS.Persistence.Repositories;

namespace WMS.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		IOrderRepository _orderRepository;

		public OrderController(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}

		[HttpGet("list")]
		public async Task<IActionResult> Get(int skip = 0, int take = 10, string orderBy = "Created", bool desc = false, bool deleted = false)
		{
			try
			{
				var result = await _orderRepository.GetCustomizedListAsync(x => x.IsDeleted == deleted, orderBy, desc, skip, take,"Customer","Lines");
				return Ok(BaseResponse<IEnumerable<Order>>.SuccessResponse(result.ToList(), "The requested list was successfuly fetched."));
			}
			catch (Exception ex)
			{
				return BadRequest(BaseResponse<IEnumerable<Order>>.ErrorResponse("An error occured while the list was fetching."));
			}

		}

		[HttpPost("add")]
		public async Task<IActionResult> Add(OrderInsertDto Order)
		{
			Order inserted = await _orderRepository.AddAsync(Order);
			if (inserted != null)
			{
				return Ok(BaseResponse<Order>.SuccessResponse(inserted, "The Order was successfully inserted into the database."));
			}
			else
			{
				return BadRequest(BaseResponse<Order>.ErrorResponse("An error occured while the Order was inserting into the database"));
			}
		}
	}
}
