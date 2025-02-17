using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Application.Dtos;
using WMS.Application.Interfaces.Repositories;
using WMS.Application.Interfaces.Services;
using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.Infrastructure.Services
{
	public class AllocationService : IAllocationService
	{
		private readonly IAllocationRepository _allocationRepository;
		private readonly ICustomerRepository _customerRepository;
		private readonly ILineRepository _lineRepository;
		private readonly ILocationRepository _locationRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly IProductRepository _productRepository;
		private readonly ISkuRepository _skuRepository;

		public AllocationService(
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

		public async Task<ServiceReturnDto<IEnumerable<Allocation>>> AllocateAllOrders()
		{
			try
			{
				var allocations = new List<Allocation>();

				// Get all orders that should be allocated
				var allOrders = await _orderRepository.GetAllAsync(
					x => x.IsDeleted == false && x.OrderStatus != OrderStatus.IsAllocated,
					"Lines"
				);
				if (allOrders == null || allOrders.Count() == 0)
				{
					return ServiceReturnDto<IEnumerable<Allocation>>.ErrorResponse("No orders to allocate.");
				}

				// Sort orders by priority and creation date
				allOrders = allOrders
					.OrderByDescending(x => x.Priority)
					.ThenBy(x => x.Created);

				foreach (var order in allOrders)
				{
					var result = await AllocateAnOrder(order);

					if (result.Data != null)
					{
						allocations.AddRange(result.Data);
					}
				}

				return ServiceReturnDto<IEnumerable<Allocation>>.SuccessResponse(allocations, "All orders allocated successfully.");
			}
			catch (Exception ex)
			{
				// Log the exception
				return ServiceReturnDto<IEnumerable<Allocation>>.ErrorResponse("An error occurred while allocating orders.");
			}
		}

		public async Task<ServiceReturnDto<IEnumerable<Allocation>>> AllocateAnOrder(Guid orderId)
		{
			var order = await _orderRepository.GetOneAsync(x => x.Id == orderId, "Lines");

			if (order == null)
			{
				return ServiceReturnDto<IEnumerable<Allocation>>.NoResultResponse("Order not found.");
			}

			return await AllocateAnOrder(order);
		}

		private async Task<ServiceReturnDto<IEnumerable<Allocation>>> AllocateAnOrder(Order order)
		{
			try
			{
				if (order.OrderStatus == OrderStatus.IsAllocated)
				{
					return ServiceReturnDto<IEnumerable<Allocation>>.NoResultResponse("The order has already been allocated.");
				}

				var skuLinePairs = new List<SkuLineHelperDto>();

				foreach (var line in order.Lines.Where(x=>x.LineStatus == LineStatus.Created || x.LineStatus == LineStatus.IsNotallocated).ToList())
				{
					var sku = await FindSuitableSkuForLine(line);

					if (sku != null)
					{
						skuLinePairs.Add(new SkuLineHelperDto(sku, line));
					}
					else if (order.CompleteDeliveryRequired == CompleteDeliveryRequired.True)
					{
						return ServiceReturnDto<IEnumerable<Allocation>>.NoResultResponse("Complete delivery required. Unable to allocate all lines.");
					}
				}
				if (skuLinePairs.Count == 0)
				{
					return ServiceReturnDto<IEnumerable<Allocation>>.NoResultResponse("No Sku Line pair founded.");
				}

				var allocations = await CreateAllocationsAndUpdateEntities(skuLinePairs);

				UpdateOrderStatus(order, allocations.Count);

				await _orderRepository.UpdateAsync(order);

				return ServiceReturnDto<IEnumerable<Allocation>>.SuccessResponse(
					allocations,
					$"Order allocated successfully. Status: {(order.OrderStatus == OrderStatus.IsAllocated ? "Fully Allocated" : "Partially Allocated")}"
				);
			}
			catch (Exception ex)
			{
				// Log the exception
				return ServiceReturnDto<IEnumerable<Allocation>>.ErrorResponse("An error occurred while allocating the order.");
			}
		}

		private async Task<Sku> FindSuitableSkuForLine(Line line)
		{
			return await _skuRepository.GetOneAsync(
				x => x.ProductId == line.ProductId &&
					 x.IsDeleted == false &&
					 x.SkuStatus != SkuStatus.Allocated &&
					 x.Quantity >= line.Quantity &&
					 x.Location.IsLocked == IsLocked.False,
				"Location"
			);
		}

		private async Task<List<Allocation>> CreateAllocationsAndUpdateEntities(List<SkuLineHelperDto> skuLinePairs)
		{
			var allocations = new List<Allocation>();

			foreach (var item in skuLinePairs)
			{
				var allocation = new Allocation
				{
					LineId = item.Line.Id,
					SkuId = item.Sku.Id,
					AllocationStatus = AllocationStatus.Allocated,
					Created = DateTime.Now
				};

				allocations.Add(allocation);

				item.Line.LineStatus = LineStatus.IsAllocated;
				item.Sku.Quantity -= item.Line.Quantity;
				item.Sku.SkuStatus = item.Sku.Quantity > 0 ? SkuStatus.NotAllocated : SkuStatus.Allocated;

				await _skuRepository.UpdateAsync(item.Sku);
				await _lineRepository.UpdateAsync(item.Line);
			}

			await _allocationRepository.InsertRangeAsync(allocations);

			return allocations;
		}

		private void UpdateOrderStatus(Order order, int allocatedLinesCount)
		{
			order.OrderStatus = allocatedLinesCount < order.Lines.Count()
				? OrderStatus.PartiallyAllocated
				: OrderStatus.IsAllocated;
		}
	}
}