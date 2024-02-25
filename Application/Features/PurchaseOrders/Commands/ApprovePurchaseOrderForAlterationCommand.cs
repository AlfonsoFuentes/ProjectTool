using Application.Features.PurchaseOrders.Validators;
using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.Approves;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Commands
{
    public record ApprovePurchaseOrderForAlterationCommand(ApprovePurchaseOrderRequest Data) : IRequest<IResult>;
    internal class ApprovePurchaseOrderForAlterationCommandHandler : IRequestHandler<ApprovePurchaseOrderForAlterationCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public ApprovePurchaseOrderForAlterationCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(ApprovePurchaseOrderForAlterationCommand request, CancellationToken cancellationToken)
        {
            var validator = new ApprovePurchaseOrderValidator(Repository);
            var validationResult = await validator.ValidateAsync(request.Data, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList());

            }

            var purchaseorder = await Repository.GetPurchaseOrderToApproveAlterationById(request.Data.PurchaseorderId);
            if (purchaseorder == null)
            {
                return Result.Fail($"Not found data approving purchase order: {request.Data.PONumber}");

            }
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Approved.Id;
            purchaseorder.PONumber = request.Data.PONumber;
            purchaseorder.POExpectedDateDate = request.Data.ExpetedOn!.Value;

            var sumPOValueUSD = request.Data.ItemsInPurchaseorder.Count == 0 ? 0 :
                   request.Data.ItemsInPurchaseorder.Sum(x => x.POValueUSD);
            foreach(var row in request.Data.ItemsInPurchaseorder)
            {
                var purchaseordertaxestem = purchaseorder.AddPurchaseOrderItemForAlteration(row.BudgetItemId,
                    $"{request.Data.PONumber} Tax {row.BudgetItemName} {purchaseorder.MWO.PercentageTaxForAlterations}%");
                purchaseordertaxestem.POValueUSD = purchaseorder.MWO.PercentageTaxForAlterations / 100.0 * row.POValueUSD;
                purchaseordertaxestem.Quantity = 1;
                await Repository.AddPurchaseorderItem(purchaseordertaxestem);
            }
            

            await Repository.UpdatePurchaseOrder(purchaseorder);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Result.Success($"Purchase order: {purchaseorder.PONumber} was approved succesfully"); ;
            }

            return Result.Fail($"Purchase order: {purchaseorder.PONumber} was not approved succesfully");
        }
    }
}
