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
	public class SkuController : ControllerBase
	{
		ISkuRepository _skuRepository;

		public SkuController(ISkuRepository skuRepository)
		{
			_skuRepository = skuRepository;
		}

		[HttpGet("list")]
		public async Task<IActionResult> Get(int skip = 0, int take = 10, string orderBy = "Created", bool desc = false, bool deleted = false)
		{
			try
			{
				var result = await _skuRepository.GetCustomizedListAsync(x => x.IsDeleted == deleted, orderBy, desc, skip, take, "Location","Product");
				return Ok(BaseResponse<IEnumerable<Sku>>.SuccessResponse(result.ToList(), "The requested list was successfuly fetched."));
			}
			catch (Exception ex)
			{
				return BadRequest(BaseResponse<IEnumerable<Sku>>.ErrorResponse("An error occured while the list was fetching."));
			}

		}

		[HttpPost("add")]
		public async Task<IActionResult> Add(SkuInsertDto Sku)
		{
			Sku inserted = await _skuRepository.AddAsync(Sku);
			if (inserted != null)
			{
				return Ok(BaseResponse<Sku>.SuccessResponse(inserted, "The Sku was successfully inserted into the database."));
			}
			else
			{
				return BadRequest(BaseResponse<Sku>.ErrorResponse("An error occured while the Sku was inserting into the database"));
			}
		}
	}
}
