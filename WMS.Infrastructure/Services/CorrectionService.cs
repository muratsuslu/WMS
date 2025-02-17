using System.Linq;
using System.Threading.Tasks;
using WMS.Application.Dtos;
using WMS.Application.Interfaces.Repositories;
using WMS.Application.Interfaces.Services;
using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.Infrastructure.Services
{
	public class CorrectionService : ICorrectionService
	{
		private readonly IAllocationRepository _allocationRepository;
		private readonly ILineRepository _lineRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly ISkuRepository _skuRepository;

		public CorrectionService(
			IAllocationRepository allocationRepository,
			ILineRepository lineRepository,
			IOrderRepository orderRepository,
			ISkuRepository skuRepository)
		{
			_allocationRepository = allocationRepository;
			_lineRepository = lineRepository;
			_orderRepository = orderRepository;
			_skuRepository = skuRepository;
		}

		public async Task<ServiceReturnDto<Sku>> CorrectSku(Guid skuId, decimal correction)
		{
			try
			{
				var sku = await _skuRepository.GetOneAsync(x => x.Id == skuId);
				if (sku == null)
					return ServiceReturnDto<Sku>.ErrorResponse("No Sku found with the requested Id.");

				var remainingDeficit = correction;
				var deallocationResult = await HandleQuantityDeficit(sku, remainingDeficit);

				sku.Quantity += correction;
				await _skuRepository.UpdateAsync(sku);

				return ServiceReturnDto<Sku>.SuccessResponse(sku, deallocationResult.Message);
			}
			catch (Exception ex)
			{
				// Log exception here
				return ServiceReturnDto<Sku>.ErrorResponse($"An error occurred: {ex.Message}");
			}
		}

		private async Task<DeallocationResult> HandleQuantityDeficit(Sku sku, decimal remainingDeficit)
		{
			var result = new DeallocationResult();

			if (remainingDeficit >= 0)
			{
				result.Message = "Sku quantity updated successfully. No deallocations needed.";
				return result;
			}

			var newQuantity = sku.Quantity + remainingDeficit;
			if (newQuantity >= 0)
			{
				result.Message = "Sku quantity updated successfully. No deallocations needed.";
				return result;
			}

			var requiredDeallocation = -newQuantity;
			var allocations = (await _allocationRepository.GetAllAsync(x => x.SkuId == sku.Id, "Line.Order"))
				.OrderByDescending(x => x.Created)
				.ToList();

			foreach (var allocation in allocations)
			{
				var allocatedQuantity = allocation.Line.Quantity;
				requiredDeallocation -= allocatedQuantity;
				result.RecordAffectedAllocation(allocation, allocatedQuantity);

				if (requiredDeallocation <= 0) break;
			}

			await PerformDeallocations(result);
			result.Message = result.AffectedItemsCount > 0
				? "Sku quantity updated with necessary deallocations."
				: "Insufficient allocations to cover deficit.";

			return result;
		}

		private async Task PerformDeallocations(DeallocationResult result)
		{
			foreach (var orderId in result.OrderIdsToCancel)
			{
				await CancelOrderAndLines(orderId);
			}

			foreach (var lineId in result.LineIdsToDeallocate)
			{
				await DeallocateLine(lineId);
			}
		}

		private async Task CancelOrderAndLines(Guid orderId)
		{
			var order = await _orderRepository.GetOneAsync(x => x.Id == orderId);
			if (order == null) return;

			order.OrderStatus = OrderStatus.Cancelled;
			await _orderRepository.UpdateAsync(order);

			var orderLines = await _lineRepository.GetAllAsync(x => x.OrderId == orderId);
			var deallocateTasks = orderLines.Select(line => DeallocateLine(line.Id));
			await Task.WhenAll(deallocateTasks);
		}

		private async Task DeallocateLine(Guid lineId)
		{
			var line = await _lineRepository.GetOneAsync(x => x.Id == lineId);
			if (line == null) return;

			line.LineStatus = LineStatus.Cancelled;
			await _lineRepository.UpdateAsync(line);
			Allocation allocation = await _allocationRepository.GetOneAsync(x => x.LineId == lineId);
			allocation.AllocationStatus = AllocationStatus.CancelledBySkuCorrection;
			await _allocationRepository.UpdateAsync(allocation);
		}

		private class DeallocationResult
		{
			public List<Guid> OrderIdsToCancel { get; } = new();
			public List<Guid> LineIdsToDeallocate { get; } = new();
			public string Message { get; set; } = string.Empty;
			public int AffectedItemsCount => OrderIdsToCancel.Count + LineIdsToDeallocate.Count;

			public void RecordAffectedAllocation(Allocation allocation, decimal quantity)
			{
				if (allocation.Line.Order.CompleteDeliveryRequired == CompleteDeliveryRequired.True)
				{
					OrderIdsToCancel.Add(allocation.Line.OrderId);
				}
				else
				{
					LineIdsToDeallocate.Add(allocation.Line.Id);
				}
			}
		}
	}
}