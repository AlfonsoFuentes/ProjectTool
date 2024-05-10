using Application.Mappers.PurchaseOrders;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.Commands
{
    public record NewPurchaseOrderCreateCommand(NewPurchaseOrderCreateRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderCreateCommandHandler : IRequestHandler<NewPurchaseOrderCreateCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderCreateCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderCreateCommand request, CancellationToken cancellationToken)
        {
            var mwo = await Repository.GetByIdAsync<MWO>(request.Data.MWOId);
            if (mwo == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.MainBudgetItem.MWOName, ResponseType.NotFound, ClassNames.MWO));
            }

            var purchaseorder = mwo.AddPurchaseOrder();

            request.Data.FromCreatePurchaseOrderRequest(purchaseorder);

           

            foreach (var item in request.Data.PurchaseOrderItems)
            {
                var purchaseorderitem = purchaseorder.AddPurchaseOrderItem(item.BudgetItemId);

                item.ToPurchaseOrderItemFromCreateRequest(purchaseorderitem);

                await Repository.AddAsync(purchaseorderitem);

                

            }
            await Repository.AddAsync(purchaseorder);
         
            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCachePurchaseOrder(purchaseorder));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseorderName, ResponseType.Created, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.Created, ClassNames.PurchaseOrders));
        }
    }
}
