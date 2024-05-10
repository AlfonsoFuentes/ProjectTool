using Application.Mappers.PurchaseOrders;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.Commands
{
    public record NewPurchaseOrderEditSalaryCommand(NewPurchaseOrderEditSalaryRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderEditCreateSalaryCommandHandler : IRequestHandler<NewPurchaseOrderEditSalaryCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderEditCreateSalaryCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderEditSalaryCommand request, CancellationToken cancellationToken)
        {
            var purchaseorder = await Repository.GetPurchaseOrderByIdCreatedAsync(request.Data.PurchaseOrderId);
            if (purchaseorder == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            
            request.Data.FromEditPurchaseOrderSalaryrequest(purchaseorder);



            foreach (var row in request.Data.PurchaseOrderItems)
            {
                foreach (var rowreceived in row.Receiveds)
                {
                    var received = await Repository.GetByIdAsync<PurchaseOrderItemReceived>(rowreceived.ReceivedId);
                    if (received != null)
                    {
                        received.CurrencyDate = DateTime.UtcNow;

                        received.USDEUR = rowreceived.USDEUR;
                        received.USDCOP = rowreceived.USDCOP;
                        received.ValueReceivedCurrency = rowreceived.ReceivedCurrency;
                        await Repository.UpdateAsync(received);
                    }

                }

            }
            await Repository.UpdateAsync(purchaseorder);

            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCachePurchaseOrder(purchaseorder));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseorderName, ResponseType.Updated, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.Updated, ClassNames.PurchaseOrders));
        }
    }
}
