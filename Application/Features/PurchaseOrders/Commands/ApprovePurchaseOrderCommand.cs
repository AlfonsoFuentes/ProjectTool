using Application.Features.PurchaseOrders.Validators;
using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.Approves;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Commands
{
    public record ApprovePurchaseOrderCommand(ApprovePurchaseOrderRequest Data) : IRequest<IResult>;


    internal class ApprovePurchaseOrderCommandHandler : IRequestHandler<ApprovePurchaseOrderCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public ApprovePurchaseOrderCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(ApprovePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var validator = new ApprovePurchaseOrderValidator(Repository);
            var validationResult = await validator.ValidateAsync(request.Data, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList());

            }

            var purchaseorder = await Repository.GetPurchaseOrderToEditById(request.Data.PurchaseorderId);
            if (purchaseorder == null)
            {
                return Result.Fail($"Not found data approving purchase order: {request.Data.PONumber}");

            }
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Approved.Id;
            purchaseorder.PONumber = request.Data.PONumber;
            purchaseorder.POExpectedDateDate = request.Data.ExpetedOn!.Value;
            if (request.Data.IsMWONoProductive && !request.Data.IsAlteration)
            {
                var TaxBudgetitem = await Repository.GetTaxBudgetItemNoProductive(purchaseorder.MWOId);
                if (TaxBudgetitem != null)
                {
                    var sumPOValueUSD = request.Data.ItemsInPurchaseorder.Count == 0 ? 0 :
                        request.Data.ItemsInPurchaseorder.Sum(x => x.POValueUSD);

                    var purchaseordertaxestem = purchaseorder.AddPurchaseOrderItemForNoProductiveTax(TaxBudgetitem.Id,
                            $"{request.Data.PONumber} Tax {TaxBudgetitem.Percentage}%");
                    purchaseordertaxestem.POValueUSD = TaxBudgetitem.Percentage / 100.0 * sumPOValueUSD;
                    await Repository.AddPurchaseorderItem(purchaseordertaxestem);

                }

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
