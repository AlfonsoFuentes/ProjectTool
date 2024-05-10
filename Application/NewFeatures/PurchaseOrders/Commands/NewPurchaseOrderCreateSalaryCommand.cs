using Application.Mappers.PurchaseOrders;
using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.Commands
{
    public record NewPurchaseOrderCreateSalaryCommand(NewPurchaseOrderCreateSalaryRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderCreateSalaryCommandHandler : IRequestHandler<NewPurchaseOrderCreateSalaryCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderCreateSalaryCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderCreateSalaryCommand request, CancellationToken cancellationToken)
        {
            var mwo = await Repository.GetByIdAsync<MWO>(request.Data.MWOId);
            if (mwo == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.MainBudgetItem.MWOName, ResponseType.NotFound, ClassNames.MWO));
            }

            var purchaseorder = mwo.AddPurchaseOrder();

            request.Data.FromCreatePurchaseOrderSalaryRequest(purchaseorder);

          

            foreach (var item in request.Data.PurchaseOrderItems)
            {
                var purchaseorderitem = purchaseorder.AddPurchaseOrderItem(item.BudgetItemId);

                item.ToPurchaseOrderItemFromReceivedRequest(purchaseorderitem);
                var received = purchaseorderitem.AddPurchaseOrderReceived();
                received.CurrencyDate = DateTime.UtcNow;
                received.USDEUR = request.Data.USDEUR;
                received.USDCOP = request.Data.USDCOP;
                received.ValueReceivedCurrency = purchaseorderitem.QuoteValueCurrency;
                await Repository.AddAsync(received);

                await Repository.AddAsync(purchaseorderitem);

               

            }
            await Repository.AddAsync(purchaseorder);

            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken,Cache.GetParamsCachePurchaseOrder(purchaseorder));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseorderName, ResponseType.Created, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.Created, ClassNames.PurchaseOrders));
        }
    }
}
