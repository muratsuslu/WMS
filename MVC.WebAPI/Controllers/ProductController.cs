using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Dtos.Product;
using WMS.Application.Interfaces.Repositories;
using WMS.Application.Wrappers;
using WMS.Domain.Entities;

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

		[HttpPost("add-a-product")]
		public async Task<IActionResult> AddAProduct(ProductInsertDto product)
		{
			Product insertedProduct = await _productRepository.AddAProduct(product);
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
