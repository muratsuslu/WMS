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
	public class LocationController : ControllerBase
	{
		ILocationRepository _locationRepository;

		public LocationController(ILocationRepository locationRepository)
		{
			_locationRepository = locationRepository;
		}

		[HttpGet("list")]
		public async Task<IActionResult> Get(int skip = 0, int take = 10, string orderBy = "Created", bool desc = false, bool deleted = false)
		{
			try
			{
				var result = await _locationRepository.GetCustomizedListAsync(x => x.IsDeleted == deleted, orderBy, desc, skip, take);
				return Ok(BaseResponse<IEnumerable<Location>>.SuccessResponse(result.ToList(), "The requested list was successfuly fetched."));
			}
			catch (Exception ex)
			{
				return BadRequest(BaseResponse<IEnumerable<Location>>.ErrorResponse("An error occured while the list was fetching."));
			}

		}

		[HttpPost("add")]
		public async Task<IActionResult> Add(LocationInsertDto Location)
		{
			Location inserted = await _locationRepository.AddAsync(Location);
			if (inserted != null)
			{
				return Ok(BaseResponse<Location>.SuccessResponse(inserted, "The Location was successfully inserted into the database."));
			}
			else
			{
				return BadRequest(BaseResponse<Location>.ErrorResponse("An error occured while the Location was inserting into the database"));
			}
		}
	}
}
