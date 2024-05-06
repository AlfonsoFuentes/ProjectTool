using Shared.NewModels.PurchaseOrders.Request;
using Shared.NewModels.PurchaseOrders.Responses;

namespace Application.NewFeatures.PurchaseOrders.Commands
{
    public record NewPurchaseOrderDeleteCommand(NewPurchaseOrderDeleteRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderDeleteCommandHandler : IRequestHandler<NewPurchaseOrderDeleteCommand, IResult>
    {
        private IAppDbContext _appDbContext;
        private IRepository Repository { get; set; }
        public NewPurchaseOrderDeleteCommandHandler(IAppDbContext appDbContext, IRepository repository)
        {
            _appDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(NewPurchaseOrderDeleteCommand request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetByIdAsync<PurchaseOrder>(request.Data.PurchaseOrderId);
            if (row == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
                await Repository.RemoveAsync(row);
           
            var result = await _appDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCachePurchaseOrderCreated(row.MWOId));

            return result > 0 ?
               Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Delete, ClassNames.PurchaseOrders)) :
               Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Delete, ClassNames.PurchaseOrders));


        }
    }
}
