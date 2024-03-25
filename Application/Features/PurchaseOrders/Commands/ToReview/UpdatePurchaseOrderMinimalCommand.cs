using Application.Features.PurchaseOrders.Validators;
using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Responses;

namespace Application.Features.PurchaseOrders.Commands.ToReview
{
    //public record UpdatePurchaseOrderMinimalCommand(UpdatePurchaseOrderMinimalRequest Data) : IRequest<IResult>;
    //internal class UpdatePurchaseOrderMinimalCommandHandler : IRequestHandler<UpdatePurchaseOrderMinimalCommand, IResult>
    //{
    //    private IPurchaseOrderRepository Repository { get; set; }
    //    private IAppDbContext AppDbContext { get; set; }
    //    public UpdatePurchaseOrderMinimalCommandHandler(IPurchaseOrderRepository repository, IAppDbContext appDbContext)
    //    {
    //        Repository = repository;
    //        AppDbContext = appDbContext;
    //    }

    //    public async Task<IResult> Handle(UpdatePurchaseOrderMinimalCommand request, CancellationToken cancellationToken)
    //    {
    //        var validator = new UpdatePurchaseOrderMinimalValidator(Repository);
    //        var validationResult = await validator.ValidateAsync(request.Data, cancellationToken);
    //        if (!validationResult.IsValid)
    //        {
    //            return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList());

    //        }
    //        var purchaseOrder = await Repository.GetPurchaseOrderById(request.Data.PurchaseOrderId);

    //        if (purchaseOrder == null)
    //        {
    //            return Result.Fail($"Purchase order :{request.Data.PurchaseOrderName} Not found");
    //        }
    //        purchaseOrder.PurchaseorderName = request.Data.PurchaseOrderName;
    //        //purchaseOrder.QuoteNo = request.Data.QuoteNo;
    //        //purchaseOrder.SupplierId = request.Data.SupplierId;
    //        //purchaseOrder.QuoteCurrency = request.Data.QuoteCurrency.Id;
    //        //purchaseOrder.USDCOP = request.Data.USDCOP;
    //        //purchaseOrder.USDEUR = request.Data.USDEUR;
    //        //purchaseOrder.MainBudgetItemId = request.Data.MainBudgetItemId;
    //        //foreach (var row in purchaseOrder.PurchaseOrderItems)
    //        //{
    //        //    if (!request.Data.PurchaseOrderItems.Any(x => x.BudgetItemId == row.BudgetItemId))
    //        //    {
    //        //        AppDbContext.PurchaseOrderItems.Remove(row);
    //        //    }
    //        //}
    //        //foreach (var item in request.Data.PurchaseOrderItems)
    //        //{
    //        //    var purchaseorderItem = await Repository.GetPurchaseOrderItemById(item.PurchaseOrderItemId);
    //        //    purchaseorderItem.Name = item.PurchaseOrderItemName;
    //        //    purchaseorderItem.Quantity = item.Quantity;
    //        //    purchaseorderItem.POValueUSD = item.UnitaryCostInUSD;
    //        //    if (item.BudgetItemId != purchaseorderItem.BudgetItemId)
    //        //        purchaseorderItem.ChangeBudgetItem(item.BudgetItemId);
    //        //    await Repository.UpdatePurchaseOrderItem(purchaseorderItem);
    //        //}


    //        await Repository.UpdatePurchaseOrder(purchaseOrder);
    //        var result = await AppDbContext.SaveChangesAsync(cancellationToken);
    //        if (result > 0)
    //        {
    //            return Result.Success($"Purchase order :{request.Data.PurchaseOrderName} was edited succesfully");
    //        }

    //        return Result.Fail($"Purchase order :{request.Data.PurchaseOrderName} was not edited succesfully");
    //    }
    //}
}
