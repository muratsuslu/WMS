using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Dtos.Sku;
using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;

namespace WMS.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SkuController : ControllerBase
	{
		private readonly ISkuRepository _skuRepository;
        public SkuController(ISkuRepository skuRepository)
        {
            _skuRepository = skuRepository;
        }

		[HttpPost("add-a-sku")]
		public async Task<IActionResult> AddASku(SkuInsertDto Sku)
		{
			Sku insertedSku = await _skuRepository.AddASku(Sku);
			if (insertedSku != null)
			{
				return Ok(insertedSku);
			}
			else
			{
				return BadRequest();
			}
		}
	}
}
