using Application.Features.PurchaseOrders.Validators;
using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Commands.ToReview
{
    //public record EditApprovedPurchaseOrderForAlterationCommand(ApprovedPurchaseOrderRequest Data) : IRequest<IResult>;
    //internal class EditApprovedPurchaseOrderForAlterationCommandHandler : IRequestHandler<EditApprovedPurchaseOrderForAlterationCommand, IResult>
    //{
    //    private IPurchaseOrderRepository Repository { get; set; }
    //    private IAppDbContext AppDbContext { get; set; }

    //    public EditApprovedPurchaseOrderForAlterationCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository)
    //    {
    //        AppDbContext = appDbContext;
    //        Repository = repository;
    //    }

    //    public async Task<IResult> Handle(EditApprovedPurchaseOrderForAlterationCommand request, CancellationToken cancellationToken)
    //    {
    //        var validator = new ApprovePurchaseOrderValidator(Repository);
    //        var validationResult = await validator.ValidateAsync(request.Data, cancellationToken);
    //        if (!validationResult.IsValid)
    //        {
    //            return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList());

    //        }

    //        var purchaseorder = await Repository.GetPurchaseOrderToApproveAlterationById(request.Data.PurchaseorderId);
    //        if (purchaseorder == null)
    //        {
    //            return Result.Fail($"Not found data approving purchase order: {request.Data.PONumber}");

    //        }

    //        purchaseorder.PONumber = request.Data.PONumber;
    //        purchaseorder.POExpectedDateDate = request.Data.ExpetedOn!.Value;

    //        var sumPOValueUSD = request.Data.PurchaseOrderItems.Count == 0 ? 0 :
    //               request.Data.PurchaseOrderItems.Sum(x => x.TotalValueUSDItem);
    //        foreach (var row in request.Data.PurchaseOrderItems)
    //        {
    //            var purchaseordertaxestem = await Repository.GetPurchaseOrderItemById(row.PurchaseOrderItemId);
    //            purchaseordertaxestem.POValueUSD = purchaseorder.MWO.PercentageTaxForAlterations / 100.0 * row.TotalValueUSDItem;
    //            purchaseordertaxestem.Quantity = 1;
    //            await Repository.UpdatePurchaseOrderItem(purchaseordertaxestem);
    //        }


    //        await Repository.UpdatePurchaseOrder(purchaseorder);
    //        var result = await AppDbContext.SaveChangesAsync(cancellationToken);
    //        if (result > 0)
    //        {
    //            return Result.Success($"Purchase order: {purchaseorder.PONumber} was updated succesfully"); ;
    //        }

    //        return Result.Fail($"Purchase order: {purchaseorder.PONumber} was not updated succesfully");
    //    }
    //}
}
