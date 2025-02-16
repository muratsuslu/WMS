using Microsoft.AspNetCore.Mvc;
using WMS.Application.Dtos;
using WMS.Application.Interfaces.Repositories;
using WMS.Application.Wrappers;
using WMS.Domain.Entities;
using WMS.Persistence.Repositories;

namespace MVC.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		IProductRepository _productRepository;

		public ProductController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		[HttpGet("list")]
		public async Task<IActionResult> Get(int skip = 0, int take = 10, string orderBy = "Created", bool desc = false, bool deleted = false)
		{
			try
			{
				var result = await _productRepository.GetCustomizedListAsync(x => x.IsDeleted == deleted, orderBy, desc, skip, take);
				return Ok(BaseResponse<IEnumerable<Product>>.SuccessResponse(result.ToList(), "The requested list was successfuly fetched."));
			}
			catch (Exception ex)
			{
				return BadRequest(BaseResponse<IEnumerable<Product>>.ErrorResponse("An error occured while the list was fetching."));
			}

		}

		[HttpPost("add")]
		public async Task<IActionResult> Add(ProductInsertDto product)
		{
			Product insertedProduct = await _productRepository.AddAsync(product);
			if (insertedProduct != null)
			{
				return Ok(BaseResponse<Product>.SuccessResponse(insertedProduct,"The product was successfully inserted into the database."));
			}
			else
			{
				return BadRequest(BaseResponse<Product>.ErrorResponse("An error occured while the product was inserting into the database"));
			}
		}

	}


}
