using Xunit;
using Moq;
using WMS.Application.Interfaces.Repositories;
using WMS.Application.Dtos.Product;
using WMS.Domain.Entities;


namespace WMS.UnitTests.Repositories.ProductRepository
{
	public class ProductRepositoryTests
	{
		private Mock<IProductRepository> _productRepositoryMock;

        public ProductRepositoryTests()
        {
			_productRepositoryMock = new Mock<IProductRepository>();

		}

		[Fact]
		
		public async Task AddAProduct_ValidProdct_ShouldCallAndSave()
		{
			var insertProduct = new ProductInsertDto()
			{
				Name = "Test Product"
			};
			var expectedProduct = new Product()
			{ 
				Name = "Test Product" 
			};

			_productRepositoryMock.Setup(repo => repo.AddAProduct(insertProduct)).ReturnsAsync(expectedProduct);

			var result = await _productRepositoryMock.Object.AddAProduct(insertProduct);

			Assert.NotNull(result);
			Assert.Equal(expectedProduct.Name, result.Name);

		}

		[Fact]
		public async Task AddAProduct_NullInput_ShouldThrowArgumentNullException()
		{
			// Arrange
			_productRepositoryMock
				.Setup(repo => repo.AddAProduct(null))
				.ThrowsAsync(new ArgumentNullException());

			// Act & Assert
			await Assert.ThrowsAsync<ArgumentNullException>(() => _productRepositoryMock.Object.AddAProduct(null));


		}
	}
}
