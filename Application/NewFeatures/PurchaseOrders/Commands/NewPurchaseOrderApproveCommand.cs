using Azure.Core;
using MediatR;
using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.Commands
{
    public record NewPurchaseOrderApproveCommand(NewPurchaseOrderApproveRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderApproveCommandHandler : IRequestHandler<NewPurchaseOrderApproveCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderApproveCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderApproveCommand request, CancellationToken cancellationToken)
        {
            var purchaseorder = await Repository.GetPurchaseOrderByIdCreatedAsync(request.Data.PurchaseOrderId);
            if (purchaseorder == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Approved.Id;
            purchaseorder.PONumber = request.Data.PurchaseOrderNumber;
            purchaseorder.POExpectedDateDate = request.Data.ExpectedDate!.Value;
            purchaseorder.USDCOP = request.Data.USDCOP;
            purchaseorder.USDEUR = request.Data.USDEUR;
            //Falta cuando la orden es de alteraciones o es no productiva

            if(request.Data.IsAlteration)
            {
               await CreateTaxesForAlterations(purchaseorder,request);
            }
            else if (!request.Data.IsAssetProductive)
            {
              
                await CreateTaxesForNonProductive(purchaseorder,request);

            }

            await Repository.UpdateAsync(purchaseorder);

            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCachePurchaseOrder(purchaseorder));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseorderName, ResponseType.Approve, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.Approve, ClassNames.PurchaseOrders));
        }
        async Task CreateTaxesForAlterations(PurchaseOrder purchaseorder,NewPurchaseOrderApproveCommand request)
        {
            foreach (var row in request.Data.PurchaseOrderItems)
            {
                var purchaseordertaxestem = purchaseorder.AddPurchaseOrderItemForAlteration(row.BudgetItemId);
                purchaseordertaxestem.Name = $"{request.Data.PurchaseOrderNumber} Tax {row.BudgetItem.Name} {purchaseorder.MWO.PercentageTaxForAlterations}%";

                var povalucurrency = purchaseorder.MWO.PercentageTaxForAlterations / 100.0 * row.ItemQuoteValueCurrency;
                purchaseordertaxestem.UnitaryValueCurrency = povalucurrency;
                purchaseordertaxestem.Quantity = 1;

                await Repository.AddAsync(purchaseordertaxestem);
            }
        }
        async Task CreateTaxesForNonProductive(PurchaseOrder purchaseorder, NewPurchaseOrderApproveCommand request)
        {
            var TaxBudgetitem = await Repository.GetTaxBudgetItemNoProductive(purchaseorder.MWOId);
            if (TaxBudgetitem != null)
            {

                var purchaseordertaxestem = purchaseorder.AddPurchaseOrderItemForNoProductiveTax(TaxBudgetitem.Id);
                purchaseordertaxestem.Name = $"{request.Data.PurchaseOrderNumber} Tax {TaxBudgetitem.Percentage}%";
                purchaseordertaxestem.UnitaryValueCurrency = TaxBudgetitem.Percentage / 100.0 * purchaseorder.QuoteValueCurrency;
                purchaseordertaxestem.Quantity = 1;
                await Repository.AddAsync(purchaseordertaxestem);

            }
        }
    }
}
