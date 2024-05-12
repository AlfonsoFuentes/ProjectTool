namespace Application.NewFeatures.PurchaseOrders.Queries
{
    public record NewPurchaseOrderGetAllClosedQuery : IRequest<IResult<NewPriorPurchaseOrderClosedResponse>>;
    internal class NewPurchaseOrderGetAllClosedQueryHandler : IRequestHandler<NewPurchaseOrderGetAllClosedQuery, IResult<NewPriorPurchaseOrderClosedResponse>>
    {
        private IQueryRepository QueryRepository { get; set; }
        private readonly IAppDbContext _cache;
        public NewPurchaseOrderGetAllClosedQueryHandler(IQueryRepository queryRepository, IAppDbContext cache)
        {
            QueryRepository = queryRepository;
            _cache = cache;
        }

        public async Task<IResult<NewPriorPurchaseOrderClosedResponse>> Handle(NewPurchaseOrderGetAllClosedQuery request, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            Func<Task<List<PurchaseOrder>>> getAll = () => QueryRepository.GetAllPurchaseOrderClosedAsync();
            try
            {
                var List = await _cache.GetOrAddAsync(Cache.GetAllPurchaseOrderClosed, getAll);
                NewPriorPurchaseOrderClosedResponse responseList = new NewPriorPurchaseOrderClosedResponse()
                {
                    PurchaseOrders = List.Select(x => x.ToPurchaseOrderResponse()).ToList(),
                };
                return Result<NewPriorPurchaseOrderClosedResponse>.Success(responseList);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Result<NewPriorPurchaseOrderClosedResponse>.Fail(message);
        }
    }
}
