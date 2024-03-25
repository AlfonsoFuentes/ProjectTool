using Application.Features.PurchaseOrders.Validators;
using Application.Features.PurchaseOrders.Validators.RegularPurchaseOrders;
using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;

namespace Application.Features.PurchaseOrders.Commands.RegularPurchaseOrders.Edits
{

    public record EditPurchaseOrderCreatedCommand(EditPurchaseOrderRegularCreatedRequestDto Data) : IRequest<IResult>;

    internal class EditPurchaseOrderCreatedCommandHandler : IRequestHandler<EditPurchaseOrderCreatedCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        public EditPurchaseOrderCreatedCommandHandler(IPurchaseOrderRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(EditPurchaseOrderCreatedCommand request, CancellationToken cancellationToken)
        {
           
            var purchaseOrder = await Repository.GetPurchaseOrderById(request.Data.PurchaseOrderId);

            if (purchaseOrder == null)
            {
                return Result.Fail($"Purchase order :{request.Data.PurchaseorderName} Not found");
            }
            purchaseOrder.PurchaseRequisition = request.Data.PurchaseRequisition;
            purchaseOrder.PurchaseorderName = request.Data.PurchaseorderName;
            purchaseOrder.QuoteNo = request.Data.QuoteNo;
            purchaseOrder.SupplierId = request.Data.SupplierId;
            purchaseOrder.QuoteCurrency = request.Data.QuoteCurrency;
            purchaseOrder.USDCOP = request.Data.USDCOP;
            purchaseOrder.USDEUR = request.Data.USDEUR;
            purchaseOrder.MainBudgetItemId = request.Data.MainBudgetItemId;
            purchaseOrder.POValueUSD = request.Data.PurchaseOrderItems.Sum(x => x.POValueUSD);
            foreach (var row in purchaseOrder.PurchaseOrderItems)
            {
                if (!request.Data.PurchaseOrderItems.Any(x => x.BudgetItemId == row.BudgetItemId))
                {
                    AppDbContext.PurchaseOrderItems.Remove(row);
                }
            }
            foreach (var item in request.Data.PurchaseOrderItems)
            {
                var purchaseorderItem = await Repository.GetPurchaseOrderItemById(item.PurchaseOrderItemId);
                if(purchaseorderItem==null)
                {
                    purchaseorderItem = purchaseOrder.AddPurchaseOrderItem(item.BudgetItemId, item.PurchaseorderItemName);
                    purchaseorderItem.POValueUSD = item.POValueUSD;
                    purchaseorderItem.Quantity = item.Quantity;
                    await Repository.AddPurchaseorderItem(purchaseorderItem);
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
