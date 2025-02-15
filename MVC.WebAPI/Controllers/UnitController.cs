using Microsoft.AspNetCore.Mvc;
using WMS.Application.Dtos.Unit;
using WMS.Application.Interfaces.Repositories;
using WMS.Application.Wrappers;
using WMS.Domain.Entities;

namespace WMS.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UnitController : ControllerBase
	{
		private readonly IUnitRepository _unitRepository;
        public UnitController(IUnitRepository unitRepository)
        {
			_unitRepository = unitRepository;
        }

		[HttpPost("add-a-unit")]
		public async Task<IActionResult> AddAUnit(UnitInsertDto unit)
		{
			Unit insertedUnit = await _unitRepository.AddAUnit(unit);
			if (insertedUnit != null)
			{
				return Ok(BaseResponse<Unit>.SuccessResponse(insertedUnit,"The unit was successfully inserted into database."));
			}
			else
			{
				return BadRequest(BaseResponse<Unit>.ErrorResponse("An error occured while the unir was inserting into database."));
			}
		}
	}
}
