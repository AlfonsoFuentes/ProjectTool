using Application.Features.PurchaseOrders.Validators.RegularPurchaseOrders;
using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Commands.RegularPurchaseOrders.Edits
{
    public record EditPurchaseOrderClosedCommand(EditPurchaseOrderRegularClosedRequestDto Data) : IRequest<IResult>;
    internal class EditPurchaseOrderClosedCommandHandler : IRequestHandler<EditPurchaseOrderClosedCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        public EditPurchaseOrderClosedCommandHandler(IPurchaseOrderRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(EditPurchaseOrderClosedCommand request, CancellationToken cancellationToken)
        {
          
            var purchaseOrder = await Repository.GetPurchaseOrderById(request.Data.PurchaseOrderId);

            if (purchaseOrder == null)
            {
                return Result.Fail($"Purchase order :{request.Data.PurchaseorderName} Not found");
            }
            purchaseOrder.PONumber = request.Data.PONumber;
            purchaseOrder.POExpectedDateDate = request.Data.ExpectedDate;
            purchaseOrder.PurchaseorderName = request.Data.PurchaseorderName;
            purchaseOrder.QuoteNo = request.Data.QuoteNo;
            purchaseOrder.SupplierId = request.Data.SupplierId;
            purchaseOrder.QuoteCurrency = request.Data.QuoteCurrency;
            purchaseOrder.USDCOP = request.Data.USDCOP;
            purchaseOrder.USDEUR = request.Data.USDEUR;
            purchaseOrder.MainBudgetItemId = request.Data.MainBudgetItemId;
            double povalueusd = purchaseOrder.POValueUSD;
            purchaseOrder.POValueUSD = request.Data.PurchaseOrderItems.Sum(x => x.POValueUSD);
            purchaseOrder.PurchaseOrderStatus = purchaseOrder.POValueUSD != povalueusd ? PurchaseOrderStatusEnum.Receiving.Id : PurchaseOrderStatusEnum.Closed.Id;
            foreach (var item in request.Data.PurchaseOrderItems)
            {
                var purchaseorderItem = await Repository.GetPurchaseOrderItemById(item.PurchaseOrderItemId);
                if (purchaseorderItem == null)
                {
                    purchaseorderItem = purchaseOrder.AddPurchaseOrderItem(item.BudgetItemId, item.PurchaseorderItemName);
                    purchaseorderItem.POValueUSD = item.POValueUSD;
                    purchaseorderItem.Quantity = item.Quantity;
                    await Repository.AddPurchaseorderItem(purchaseorderItem);
                    if (purchaseOrder.IsAlteration)
                    {
                        var purchaseordertaxestem = purchaseOrder.AddPurchaseOrderItemForAlteration(item.BudgetItemId,
                   $"{request.Data.PONumber} Tax {item.BudgetItemName} {purchaseOrder.MWO.PercentageTaxForAlterations}%");
                        purchaseordertaxestem.POValueUSD = purchaseOrder.MWO.PercentageTaxForAlterations / 100.0 * item.POValueUSD;
                        purchaseordertaxestem.Quantity = 1;

                        await Repository.AddPurchaseorderItem(purchaseordertaxestem);

                    }
                    await AppDbContext.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    purchaseorderItem.Name = item.PurchaseorderItemName;
                    purchaseorderItem.Quantity = item.Quantity;
                    purchaseorderItem.POValueUSD = item.POValueUSD;
                    if (item.BudgetItemId != purchaseorderItem.BudgetItemId)
                        purchaseorderItem.ChangeBudgetItem(item.BudgetItemId);
                    await Repository.UpdatePurchaseOrderItem(purchaseorderItem);
                }

            }
            var sumPOValueUSD = request.Data.PurchaseOrderItems.Count == 0 ? 0 :
                  request.Data.PurchaseOrderItems.Sum(x => x.POValueUSD);

            if (purchaseOrder.IsAlteration)
            {
                foreach (var row in request.Data.PurchaseOrderItems)
                {
                    var alterationsitem = await Repository.GetPurchaseOrderItemsAlterationsById(purchaseOrder.Id, row.BudgetItemId);
                    if (alterationsitem != null)
                    {
                        alterationsitem.POValueUSD = row.POValueUSD * purchaseOrder.MWO.PercentageTaxForAlterations / 100;
                        await Repository.UpdatePurchaseOrderItem(alterationsitem);
                        sumPOValueUSD += alterationsitem.POValueUSD;
                    }

                }

            }
            foreach (var row in purchaseOrder.PurchaseOrderItems)
            {
                if (!request.Data.PurchaseOrderItems.Any(x => x.BudgetItemId == row.BudgetItemId))
                {
                    AppDbContext.PurchaseOrderItems.Remove(row);
                }
            }



            await Repository.UpdatePurchaseOrder(purchaseOrder);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Result.Success($"Purchase order :{request.Data.PurchaseorderName} was edited succesfully");
            }

            return Result.Fail($"Purchase order :{request.Data.PurchaseorderName} was not edited succesfully");
        }
    }
}
