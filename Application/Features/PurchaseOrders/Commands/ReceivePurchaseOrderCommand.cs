using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.Receives;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Commands
{
    public record ReceivePurchaseOrderCommand(ReceivePurchaseOrderRequest Data) : IRequest<IResult>;
    internal class ReceivePurchaseOrderCommandHandler : IRequestHandler<ReceivePurchaseOrderCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public ReceivePurchaseOrderCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(ReceivePurchaseOrderCommand request, CancellationToken cancellationToken)
        {

            var purchaseorder = await Repository.GetPurchaseOrderById(request.Data.PurchaseorderId);
            if (purchaseorder == null)
            {
                return Result.Fail($"Not found data approving purchase order: {request.Data.PONumber}");

            }
            purchaseorder.PurchaseOrderStatus = request.Data.SumPendingUSD == 0 ? PurchaseOrderStatusEnum.Closed.Id : PurchaseOrderStatusEnum.Receiving.Id;
            purchaseorder.Actual = request.Data.SumActualUSD;

            foreach (var row in request.Data.ItemsInPurchaseorder)
            {
                var item = await Repository.GetPurchaseOrderItemById(row.PurchaseOrderItemId);
                if (item != null)
                {
                    item.Actual = row.ActualUSD;
                    await Repository.UpdatePurchaseOrderItem(item);
                }

            }
            if (request.Data.IsNoAssetProductive)
            {
                var PurchaseorderItemTax = await Repository.GetPurchaseOrderItemForTaxesItemById(purchaseorder.Id);
                if (PurchaseorderItemTax != null)
                {
                    var sumActualPOUSD = request.Data.SumActualUSD;
                    PurchaseorderItemTax.Actual = sumActualPOUSD * PurchaseorderItemTax.BudgetItem.Percentage / 100;
                    await Repository.UpdatePurchaseOrderItem(PurchaseorderItemTax);
                    
                }
                

            }

            await Repository.UpdatePurchaseOrder(purchaseorder);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Result.Success($"Purchase order: {purchaseorder.PONumber} was received succesfully"); ;
            }

            return Result.Fail($"Purchase order: {purchaseorder.PONumber} was not received succesfully");
        }
    }
}
