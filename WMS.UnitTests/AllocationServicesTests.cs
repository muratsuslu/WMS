using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using WMS.Application.Interfaces.Repositories;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Infrastructure.Services;

namespace WMS.Infrastructure.Tests.Services
{
	public class AllocationServiceTests
	{
		private readonly Mock<IAllocationRepository> _mockAllocationRepository;
		private readonly Mock<ICustomerRepository> _mockCustomerRepository;
		private readonly Mock<ILineRepository> _mockLineRepository;
		private readonly Mock<ILocationRepository> _mockLocationRepository;
		private readonly Mock<IOrderRepository> _mockOrderRepository;
		private readonly Mock<IProductRepository> _mockProductRepository;
		private readonly Mock<ISkuRepository> _mockSkuRepository;

		private readonly AllocationService _service;

		public AllocationServiceTests()
		{
			_mockAllocationRepository = new Mock<IAllocationRepository>();
			_mockCustomerRepository = new Mock<ICustomerRepository>();
			_mockLineRepository = new Mock<ILineRepository>();
			_mockLocationRepository = new Mock<ILocationRepository>();
			_mockOrderRepository = new Mock<IOrderRepository>();
			_mockProductRepository = new Mock<IProductRepository>();
			_mockSkuRepository = new Mock<ISkuRepository>();

			_service = new AllocationService(
				_mockAllocationRepository.Object,
				_mockCustomerRepository.Object,
				_mockLineRepository.Object,
				_mockLocationRepository.Object,
				_mockOrderRepository.Object,
				_mockProductRepository.Object,
				_mockSkuRepository.Object
			);
		}

		[Fact]
		public async Task AllocateAllOrders_NoOrders_ReturnsNoResultResponse()
		{
			// Arrange
			_mockOrderRepository
				.Setup(repo => repo.GetAllAsync(
					It.IsAny<Expression<Func<Order, bool>>>(),
					It.IsAny<string>()))
				.ReturnsAsync(new List<Order>());

			// Act
			var result = await _service.AllocateAllOrders();

			// Assert
			result.Should().NotBeNull();
			result.IsSuccess.Should().BeFalse();
			result.Message.Should().Contain("No orders to allocate.");
		}

		[Fact]
		public async Task AllocateAllOrders_OrdersAllocatedSuccessfully_ReturnsSuccessResponse()
		{
			// Arrange
			var orders = new List<Order>
			{
				new Order { Id = Guid.NewGuid(), Priority = OrderPriority.High, Created = DateTime.Now, Lines = new List<Line>() }
			};

			_mockOrderRepository
				.Setup(repo => repo.GetAllAsync(
					It.IsAny<Expression<Func<Order, bool>>>(),
					It.IsAny<string>()))
				.ReturnsAsync(orders);

			_mockSkuRepository
				.Setup(repo => repo.GetOneAsync(
					It.IsAny<Expression<Func<Sku, bool>>>(),
					It.IsAny<string>()))
				.ReturnsAsync(new Sku { Quantity = 100 });

			// Act
			var result = await _service.AllocateAllOrders();

			// Assert
			result.Should().NotBeNull();
			result.IsSuccess.Should().BeTrue();
			//result.Data.Should().NotBeEmpty();
		}

		[Fact]
		public async Task AllocateAnOrder_OrderNotFound_ReturnsNoResultResponse()
		{
			// Arrange
			_mockOrderRepository
				.Setup(repo => repo.GetOneAsync(
					It.IsAny<Expression<Func<Order, bool>>>(),
					It.IsAny<string>()))
				.ReturnsAsync((Order)null);

			// Act
			var result = await _service.AllocateAnOrder(Guid.NewGuid());

			// Assert
			result.Should().NotBeNull();
			//result.IsSuccess.Should().BeFalse();
			result.Message.Should().Contain("Order not found.");
		}

		[Fact]
		public async Task AllocateAnOrder_OrderAlreadyAllocated_ReturnsNoResultResponse()
		{
			// Arrange
			var order = new Order { Id = Guid.NewGuid(), OrderStatus = OrderStatus.IsAllocated };

			_mockOrderRepository
				.Setup(repo => repo.GetOneAsync(
					It.IsAny<Expression<Func<Order, bool>>>(),
					It.IsAny<string>()))
				.ReturnsAsync(order);

			// Act
			var result = await _service.AllocateAnOrder(order.Id);

			// Assert
			result.Should().NotBeNull();
			//result.IsSuccess.Should().BeFalse();
			result.Message.Should().Contain("already been allocated");
		}

		[Fact]
		public async Task AllocateAnOrder_CompleteDeliveryRequiredButInsufficientStock_ReturnsNoResultResponse()
		{
			// Arrange
			var order = new Order
			{
				Id = Guid.NewGuid(),
				CompleteDeliveryRequired = CompleteDeliveryRequired.True,
				Lines = new List<Line> { new Line { Quantity = 100 } }
			};

			_mockOrderRepository
				.Setup(repo => repo.GetOneAsync(
					It.IsAny<Expression<Func<Order, bool>>>(),
					It.IsAny<string>()))
				.ReturnsAsync(order);

			_mockSkuRepository
				.Setup(repo => repo.GetOneAsync(
					It.IsAny<Expression<Func<Sku, bool>>>(),
					It.IsAny<string>()))
				.ReturnsAsync((Sku)null);

			// Act
			var result = await _service.AllocateAnOrder(order.Id);

			// Assert
			result.Should().NotBeNull();
			//result.IsSuccess.Should().BeFalse();
			result.Message.Should().Contain("Complete delivery required");
		}

		[Fact]
		public async Task AllocateAnOrder_OrderAllocatedSuccessfully_ReturnsSuccessResponse()
		{
			// Arrange
			var order = new Order
			{
				Id = Guid.NewGuid(),
				Lines = new List<Line> { new Line { Quantity = 10, ProductId = Guid.NewGuid() } }
			};

			var sku = new Sku { Id = Guid.NewGuid(), Quantity = 100, ProductId = order.Lines.First().ProductId };

			_mockOrderRepository
				.Setup(repo => repo.GetOneAsync(
					It.IsAny<Expression<Func<Order, bool>>>(),
					It.IsAny<string>()))
				.ReturnsAsync(order);

			_mockSkuRepository
				.Setup(repo => repo.GetOneAsync(
					It.IsAny<Expression<Func<Sku, bool>>>(),
					It.IsAny<string>()))
				.ReturnsAsync(sku);

			// Act
			var result = await _service.AllocateAnOrder(order.Id);

			// Assert
			result.Should().NotBeNull();
			result.IsSuccess.Should().BeTrue();
			result.Data.Should().NotBeEmpty();
		}
	}
}