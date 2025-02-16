using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WMS.Application.Dtos;
using WMS.Domain.Entities;
using WMS.Persistence.Context;
using WMS.Persistence.Repositories;
using Xunit;

namespace WMS.Tests.Repositories
{
	public class RepositoryTests
	{
		private readonly Mock<ApplicationDbContext> _mockContext;
		private readonly Mock<IMapper> _mockMapper;
		private readonly Repository<Customer> _repository;
		private readonly List<Customer> _sampleCustomers;

		public RepositoryTests()
		{
			// DbContextOptions'u oluştur
			var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			// ApplicationDbContext'i mock'la
			_mockContext = new Mock<ApplicationDbContext>(dbContextOptions);
			_mockMapper = new Mock<IMapper>();
			_repository = new Repository<Customer>(_mockContext.Object, _mockMapper.Object);

			// Örnek veriler
			_sampleCustomers = new List<Customer>
			{
				new Customer { Id = Guid.NewGuid(), FullName = "Customer 1", Email = "c1@test.com" },
				new Customer { Id = Guid.NewGuid(), FullName = "Customer 2", Email = "c2@test.com" }
			};

			// Mock DbSet
			var mockDbSet = GetMockDbSet(_sampleCustomers);
			_mockContext.Setup(c => c.Set<Customer>()).Returns(mockDbSet.Object);
		}

		private static Mock<DbSet<T>> GetMockDbSet<T>(List<T> data) where T : class
		{
			var queryable = data.AsQueryable();
			var mockDbSet = new Mock<DbSet<T>>();
			mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
			mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
			mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
			mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
			return mockDbSet;
		}

		[Fact]
		public async Task GetAllAsync_WithoutFilter_ReturnsAllCustomers()
		{
			// Act
			var result = await _repository.GetAllAsync();

			// Assert
			Assert.Equal(2, result.Count());
		}

		[Fact]
		public async Task GetOneAsync_WithValidFilter_ReturnsCustomer()
		{
			// Arrange
			Expression<Func<Customer, bool>> filter = c => c.FullName == "Customer 1";

			// Act
			var result = await _repository.GetOneAsync(filter);

			// Assert
			Assert.NotNull(result);
			Assert.Equal("Customer 1", result.FullName);
		}

		[Fact]
		public async Task InsertAsync_ValidEntity_AddsToDbContext()
		{
			// Arrange
			var newCustomer = new Customer { FullName = "New Customer" };
			_mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

			// Act
			var result = await _repository.InsertAsync(newCustomer);

			// Assert
			_mockContext.Verify(c => c.AddAsync(newCustomer, default), Times.Once);
			_mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
			Assert.Equal("New Customer", result.FullName);
		}

		[Fact]
		public async Task UpdateAsync_ValidEntity_SetsUpdatedDate()
		{
			// Arrange
			var customer = _sampleCustomers.First();
			var originalUpdatedDate = customer.Updated;

			// Act
			var result = await _repository.UpdateAsync(customer);

			// Assert
			Assert.True(result.Updated > originalUpdatedDate);
			_mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
		}

		[Fact]
		public async Task AddAsync_WithDto_MapsAndInsertsEntity()
		{
			// Arrange
			var dto = new CustomerInsertDto { FullName = "DTO Customer", Email = "dto@test.com" };
			var mappedCustomer = new Customer { FullName = dto.FullName, Email = dto.Email };

			_mockMapper.Setup(m => m.Map<Customer>(dto)).Returns(mappedCustomer);
			_mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

			// Act
			var result = await _repository.AddAsync(dto);

			// Assert
			_mockMapper.Verify(m => m.Map<Customer>(dto), Times.Once);
			_mockContext.Verify(c => c.AddAsync(mappedCustomer, default), Times.Once);
			Assert.Equal("DTO Customer", result.FullName);
		}

		[Fact]
		public async Task DeleteAsync_ValidEntity_RemovesFromDbContext()
		{
			// Arrange
			var customer = _sampleCustomers.First();

			// Act
			await _repository.DeleteAsync(customer);

			// Assert
			_mockContext.Verify(c => c.Remove(customer), Times.Once);
		}

		[Fact]
		public async Task GetCustomizedListAsync_AppliesPagination()
		{
			// Act
			var result = await _repository.GetCustomizedListAsync(skip: 1, take: 1);

			// Assert
			Assert.Single(result);
		}
	}
}