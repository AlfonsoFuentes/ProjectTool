using Application.Caches;
using Application.Mappers.PurchaseOrders;
using Shared.NewModels.PurchaseOrders.Responses;

namespace Application.NewFeatures.PurchaseOrders.Queries
{
    public record NewPurchaseOrderGetAllCreatedQuery:IRequest<IResult<NewPriorPurchaseOrderCreatedResponse>>;

    internal class NewPurchaseOrderGetAllCreatedQueryHandler : IRequestHandler<NewPurchaseOrderGetAllCreatedQuery, IResult<NewPriorPurchaseOrderCreatedResponse>>
    {
        private IQueryRepository QueryRepository { get; set; }
        private readonly IAppDbContext _cache;
        public NewPurchaseOrderGetAllCreatedQueryHandler(IQueryRepository queryRepository, IAppDbContext cache)
        {
            QueryRepository = queryRepository;
            _cache = cache;
        }

        public async Task<IResult<NewPriorPurchaseOrderCreatedResponse>> Handle(NewPurchaseOrderGetAllCreatedQuery request, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            Func<Task<List<PurchaseOrder>>> getAll = () => QueryRepository.GetAllPurchaseOrderCreatedAsync();
            try
            {
                var List = await _cache.GetOrAddAsync(Cache.GetAllPurchaseOrderCreated, getAll);
                NewPriorPurchaseOrderCreatedResponse responseList = new NewPriorPurchaseOrderCreatedResponse()
                {
                    PurchaseOrders = List.Select(x => x.ToPurchaseOrderResponse()).ToList(),
                };
                return Result<NewPriorPurchaseOrderCreatedResponse>.Success(responseList);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Result<NewPriorPurchaseOrderCreatedResponse>.Fail(message);
        }
    }
}
