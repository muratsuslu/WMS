using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using WMS.Application.Dtos;
using WMS.Application.Interfaces.Repositories;
using WMS.Application.Interfaces.Services;
using WMS.Domain.Entities;
using WMS.Domain.Enums;
using WMS.Infrastructure.Services;
xUnit;

public class CancellationServiceTests
{
	private readonly Mock<IAllocationRepository> _allocationRepositoryMock = new();
	private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
	private readonly Mock<ILineRepository> _lineRepositoryMock = new();
	private readonly Mock<ILocationRepository> _locationRepositoryMock = new();
	private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
	private readonly Mock<IProductRepository> _productRepositoryMock = new();
	private readonly Mock<ISkuRepository> _skuRepositoryMock = new();
	private readonly ICancellationService _service;

	public CancellationServiceTests()
	{
		_service = new ICancellationService(
			_allocationRepositoryMock.Object,
			_customerRepositoryMock.Object,
			_lineRepositoryMock.Object,
			_locationRepositoryMock.Object,
			_orderRepositoryMock.Object,
			_productRepositoryMock.Object,
			_skuRepositoryMock.Object);
	}

	[Fact]
	public async Task CancelAnOrder_ShouldReturnError_WhenOrderNotFound()
	{
		// Arrange
		_orderRepositoryMock.Setup(repo => repo.GetOneAsync(It.IsAny<Func<Order, bool>>(), "Lines"))
			.ReturnsAsync((Order)null);

		// Act
		var result = await _service.CancelAnOrder(Guid.NewGuid());

		// Assert
		result.Success.Should().BeFalse();
		result.Message.Should().Be("Order not found.");
	}

	[Fact]
	public async Task CancelAnOrder_ShouldReturnError_WhenOrderAlreadyCancelled()
	{
		// Arrange
		var order = new Order { Id = Guid.NewGuid(), OrderStatus = OrderStatus.Cancelled };
		_orderRepositoryMock.Setup(repo => repo.GetOneAsync(It.IsAny<Func<Order, bool>>(), "Lines"))
			.ReturnsAsync(order);

		// Act
		var result = await _service.CancelAnOrder(order.Id);

		// Assert
		result.Success.Should().BeFalse();
		result.Message.Should().Be("The order has already been cancelled.");
	}

	[Fact]
	public async Task CancelAnOrder_ShouldCancelOrder_WhenValid()
	{
		// Arrange
		var order = new Order { Id = Guid.NewGuid(), OrderStatus = OrderStatus.Pending, Lines = new List<Line>() };
		_orderRepositoryMock.Setup(repo => repo.GetOneAsync(It.IsAny<Func<Order, bool>>(), "Lines"))
			.ReturnsAsync(order);
		_orderRepositoryMock.Setup(repo => repo.UpdateAsync(order)).Returns(Task.CompletedTask);

		// Act
		var result = await _service.CancelAnOrder(order.Id);

		// Assert
		result.Success.Should().BeTrue();
		result.Data.Should().NotBeNull();
		result.Data.OrderStatus.Should().Be(OrderStatus.Cancelled);
		result.Message.Should().Be("The order was successfully cancelled.");
	}
}
