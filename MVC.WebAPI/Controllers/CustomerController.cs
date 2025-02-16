using Microsoft.AspNetCore.Http;
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
	public class CustomerController : ControllerBase
	{
		ICustomerRepository _customerRepository;

		public CustomerController(ICustomerRepository customerRepository)
		{
			_customerRepository = customerRepository;
		}

		[HttpGet("list")]
		public async Task<IActionResult> Get(int skip = 0, int take = 10, string orderBy = "Created", bool desc = false, bool deleted = false)
		{
			try
			{
				var result = await _customerRepository.GetCustomizedListAsync(x => x.IsDeleted == deleted, orderBy, desc, skip, take, "Orders");
				return Ok(BaseResponse<IEnumerable<Customer>>.SuccessResponse(result.ToList(), "The requested list was successfuly fetched."));
			}
			catch (Exception ex)
			{
				return BadRequest(BaseResponse<IEnumerable<Customer>>.ErrorResponse("An error occured while the list was fetching."));
			}

		}

		[HttpPost("add")]
		public async Task<IActionResult> Add(CustomerInsertDto Customer)
		{
			Customer inserted = await _customerRepository.AddAsync(Customer);
			if (inserted != null)
			{
				return Ok(BaseResponse<Customer>.SuccessResponse(inserted, "The Customer was successfully inserted into the database."));
			}
			else
			{
				return BadRequest(BaseResponse<Customer>.ErrorResponse("An error occured while the Customer was inserting into the database"));
			}
		}
	}
}
