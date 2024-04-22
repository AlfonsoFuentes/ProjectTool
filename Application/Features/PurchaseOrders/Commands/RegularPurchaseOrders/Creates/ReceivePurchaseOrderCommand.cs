﻿using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Commands.RegularPurchaseOrders.Creates
{
    public record ReceivePurchaseOrderCommand(ReceiveRegularPurchaseOrderRequest Data) : IRequest<IResult>;
    internal class ReceivePurchaseOrderCommandHandler : IRequestHandler<ReceivePurchaseOrderCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        private IMWORepository MWORepository { get; set; }
        public ReceivePurchaseOrderCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository, IMWORepository mWORepository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(ReceivePurchaseOrderCommand request, CancellationToken cancellationToken)
        {

            var purchaseorder = await Repository.GetPurchaseOrderById(request.Data.PurchaseOrderId);
            if (purchaseorder == null)
            {
                return Result.Fail($"Not found data approving purchase order: {request.Data.PONumber}");

            }
            purchaseorder.PurchaseOrderStatus = request.Data.SumPONewPendingCurrency == 0 ? PurchaseOrderStatusEnum.Closed.Id : PurchaseOrderStatusEnum.Receiving.Id;
            var sumPOactualCurrency = request.Data.SumPONewActualCurrency;
            purchaseorder.POClosedDate = purchaseorder.PurchaseOrderStatus == PurchaseOrderStatusEnum.Closed.Id ? DateTime.UtcNow : null;
            foreach (var row in request.Data.PurchaseOrderItemsToReceive)
            {
                var item = await Repository.GetPurchaseOrderItemById(row.PurchaseOrderItemId);
                if (item != null)
                {
                    item.ActualCurrency= row.PONewActualCurrency;
                    await Repository.UpdatePurchaseOrderItem(item);
                }

            }
            if (!request.Data.IsAssetProductive && !request.Data.IsAlteration)
            {
                var PurchaseorderItemTax = await Repository.GetPurchaseOrderItemForTaxesItemById(purchaseorder.Id);
                if (PurchaseorderItemTax != null)
                {
                    var sumTaxPOCurrency = request.Data.SumPONewActualCurrency;
                    sumPOactualCurrency += sumTaxPOCurrency;
                    PurchaseorderItemTax.ActualCurrency = sumTaxPOCurrency * PurchaseorderItemTax.BudgetItem.Percentage / 100;
                    await Repository.UpdatePurchaseOrderItem(PurchaseorderItemTax);

                }
            }
            if (request.Data.IsAlteration)
            {
                foreach (var row in request.Data.PurchaseOrderItemsToReceive)
                {
                    var purchaseorderItemTaxForAlteration = await Repository.GetPurchaseOrderItemForTaxesForAlterationById(purchaseorder.Id, row.BudgetItemId);
                    if (purchaseorderItemTaxForAlteration != null)
                    {
                        var TaxAlterationCurrency= row.PONewActualCurrency * request.Data.PercentageAlteration / 100.0;
                        sumPOactualCurrency += TaxAlterationCurrency;
                        purchaseorderItemTaxForAlteration.ActualCurrency = TaxAlterationCurrency;
                        await Repository.UpdatePurchaseOrderItem(purchaseorderItemTaxForAlteration);
                    }

                }

            }

            //purchaseorder.ActualCurrency = sumPOactualCurrency;
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
