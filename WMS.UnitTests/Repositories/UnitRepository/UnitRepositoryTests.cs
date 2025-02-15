using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Application.Dtos.Unit;
using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;

namespace WMS.UnitTests.Repositories.UnitRepository
{
	public class UnitRepositoryTests
	{
		private Mock<IUnitRepository> _UnitRepositoryMock;

		public UnitRepositoryTests()
		{
			_UnitRepositoryMock = new Mock<IUnitRepository>();

		}

		[Fact]

		public async Task AddAUnit_ValidProdct_ShouldCallAndSave()
		{
			var insertUnit = new UnitInsertDto()
			{
				Name = "Test Unit"
			};
			var expectedUnit = new Unit()
			{
				Name = "Test Unit"
			};

			_UnitRepositoryMock.Setup(repo => repo.AddAUnit(insertUnit)).ReturnsAsync(expectedUnit);

			var result = await _UnitRepositoryMock.Object.AddAUnit(insertUnit);

			Assert.NotNull(result);
			Assert.Equal(expectedUnit.Name, result.Name);

		}

		[Fact]
		public async Task AddAUnit_NullInput_ShouldThrowArgumentNullException()
		{
			// Arrange
			_UnitRepositoryMock
				.Setup(repo => repo.AddAUnit(null))
				.ThrowsAsync(new ArgumentNullException());

			// Act & Assert
			await Assert.ThrowsAsync<ArgumentNullException>(() => _UnitRepositoryMock.Object.AddAUnit(null));


		}
	}
}
