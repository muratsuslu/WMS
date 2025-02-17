using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Application.Dtos;
using WMS.Application.Interfaces.Repositories;
using WMS.Application.Interfaces.Services;
using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.Infrastructure.Services
{
	internal class CancellationService : ICancellationService
	{
		private readonly IAllocationRepository _allocationRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly ILineRepository _lineRepository;
		private readonly ILocationRepository _locationRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly IProductRepository _productRepository;
		private readonly ISkuRepository _skuRepository;

		public CancellationService(
			IAllocationRepository allocationRepository,
			ICustomerRepository customerRepository,
			ILineRepository lineRepository,
			ILocationRepository locationRepository,
			IOrderRepository orderRepository,
			IProductRepository productRepository,
			ISkuRepository skuRepository)
		{
			_allocationRepository = allocationRepository;
			_customerRepository = customerRepository;
			_lineRepository = lineRepository;
			_locationRepository = locationRepository;
			_orderRepository = orderRepository;
			_productRepository = productRepository;
			_skuRepository = skuRepository;
		}

		public async Task<ServiceReturnDto<Order>> CancelAnOrder(Guid orderId)
		{
			try
			{
				var order = await FetchOrderAsync(orderId);
				if (order == null)
				{
					return ServiceReturnDto<Order>.NoResultResponse("Order not found.");
				}

				if (order.OrderStatus == OrderStatus.Cancelled)
				{
					return ServiceReturnDto<Order>.NoResultResponse("The order has already been cancelled.");
				}

				order.OrderStatus = OrderStatus.Cancelled;

				var (allocations, skus) = await CancelOrderLinesAndDeallocateSkusAsync(order);

				await UpdateEntitiesAsync(order, allocations, skus);

				return ServiceReturnDto<Order>.SuccessResponse(order, "The order was successfully cancelled.");
			}
			catch (Exception ex)
			{
				// Log the exception (e.g., using ILogger)
				return ServiceReturnDto<Order>.ErrorResponse("An error occurred while cancelling the order.");
			}
		}

		public async Task<ServiceReturnDto<Line>> CancelASingleLine(Guid lineId)
		{
			try
			{
				var line = await FetchLineAsync(lineId);
				if (line == null)
				{
					return ServiceReturnDto<Line>.ErrorResponse("No line with the requested ID found.");
				}

				if (line.LineStatus == LineStatus.Cancelled)
				{
					return ServiceReturnDto<Line>.ErrorResponse("The line has already been cancelled.");
				}

				line.LineStatus = LineStatus.Cancelled;

				var (allocation, sku) = await DeallocateSkuForLineAsync(line);
				if (allocation == null || sku == null)
				{
					return ServiceReturnDto<Line>.NoResultResponse("No allocation or SKU found for the line.");
				}

				await UpdateEntitiesAsync(line, allocation, sku);

				return ServiceReturnDto<Line>.SuccessResponse(line, "The line was successfully cancelled.");
			}
			catch (Exception ex)
			{
				// Log the exception (e.g., using ILogger)
				return ServiceReturnDto<Line>.ErrorResponse("An error occurred while cancelling the line.");
			}
		}

		private async Task<Order> FetchOrderAsync(Guid orderId)
		{
			return await _orderRepository.GetOneAsync(x => x.Id == orderId, "Lines");
		}

		private async Task<Line> FetchLineAsync(Guid lineId)
		{
			return await _lineRepository.GetOneAsync(x => x.Id == lineId);
		}

		private async Task<(List<Allocation>, List<Sku>)> CancelOrderLinesAndDeallocateSkusAsync(Order order)
		{
			var allocations = new List<Allocation>();
			var skus = new List<Sku>();

			foreach (var line in order.Lines)
			{
				line.LineStatus = LineStatus.Cancelled;

				var allocation = await FetchAllocationForLineAsync(line.Id);
				if (allocation != null)
				{
					allocation.AllocationStatus = AllocationStatus.CancelledByUser;

					var sku = await DeallocateSkuAsync(allocation.Sku, line.Quantity);
					skus.Add(sku);
					allocations.Add(allocation);
				}
			}

			return (allocations, skus);
		}

		private async Task<Allocation> FetchAllocationForLineAsync(Guid lineId)
		{
			return await _allocationRepository.GetOneAsync(x => x.LineId == lineId, "Sku");
		}

		private async Task<Sku> DeallocateSkuAsync(Sku sku, decimal quantity)
		{
			sku.SkuStatus = SkuStatus.NotAllocated;
			sku.Quantity += quantity;
			return sku;
		}

		private async Task<(Allocation, Sku)> DeallocateSkuForLineAsync(Line line)
		{
			var allocation = await FetchAllocationForLineAsync(line.Id);
			if (allocation == null)
			{
				return (null, null);
			}

			var sku = await DeallocateSkuAsync(allocation.Sku, line.Quantity);
			return (allocation, sku);
		}

		private async Task UpdateEntitiesAsync(Order order, List<Allocation> allocations, List<Sku> skus)
		{
			await _orderRepository.UpdateAsync(order);
			await _skuRepository.UpdateRangeAsync(skus);
			await _allocationRepository.UpdateRangeAsync(allocations);
			await _lineRepository.UpdateRangeAsync(order.Lines);
		}

		private async Task UpdateEntitiesAsync(Line line, Allocation allocation, Sku sku)
		{
			await _lineRepository.UpdateAsync(line);
			await _allocationRepository.UpdateAsync(allocation);
			await _skuRepository.UpdateAsync(sku);
		}
	}
}