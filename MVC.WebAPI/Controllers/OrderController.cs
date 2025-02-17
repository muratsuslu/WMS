using Microsoft.AspNetCore.Mvc;
using WMS.Application.Dtos;
using WMS.Application.Interfaces.Repositories;
using WMS.Application.Interfaces.Services;
using WMS.Application.Wrappers;
using WMS.Domain.Entities;
using WMS.Persistence.Repositories;

namespace WMS.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		protected readonly IOrderRepository _orderRepository;
		protected readonly ICancellationService _orderCancellationService;

		public OrderController(IOrderRepository orderRepository, ICancellationService orderCancellationService)
		{
			_orderRepository = orderRepository;
			_orderCancellationService = orderCancellationService;
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

		[HttpPost("cancel-an-order")]
		public async Task<IActionResult> Cancel(Guid orderId)
		{
			var result = await _orderCancellationService.CancelAnOrder(orderId);
			if (result != null)
			{
				return Ok(ServiceReturnDto<Order>.SuccessResponse(result.Data, "The Order was successfully inserted into the database."));
			}
			else
			{
				return BadRequest(ServiceReturnDto<Order>.ErrorResponse("An error occured while the Order was inserting into the database"));
			}
		}

		[HttpPost("cancel-a-line")]
		public async Task<IActionResult> CancelALine(Guid lineId)
		{
			var result = await _orderCancellationService.CancelASingleLine(lineId);
			if (result != null)
			{
				return Ok(ServiceReturnDto<Line>.SuccessResponse(result.Data, "The Order was successfully inserted into the database."));
			}
			else
			{
				return BadRequest(ServiceReturnDto<Line>.ErrorResponse("An error occured while the Order was inserting into the database"));
			}
		}
	}
}
