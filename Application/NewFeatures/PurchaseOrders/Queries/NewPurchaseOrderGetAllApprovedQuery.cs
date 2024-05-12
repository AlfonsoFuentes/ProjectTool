namespace Application.NewFeatures.PurchaseOrders.Queries
{
    public record NewPurchaseOrderGetAllApprovedQuery : IRequest<IResult<NewPriorPurchaseOrderApprovedResponse>>;
    internal class NewPurchaseOrderGetAllApprovedQueryHandler : IRequestHandler<NewPurchaseOrderGetAllApprovedQuery, IResult<NewPriorPurchaseOrderApprovedResponse>>
    {
        private IQueryRepository QueryRepository { get; set; }
        private readonly IAppDbContext _cache;
        public NewPurchaseOrderGetAllApprovedQueryHandler(IQueryRepository queryRepository, IAppDbContext cache)
        {
            QueryRepository = queryRepository;
            _cache = cache;
        }

        public async Task<IResult<NewPriorPurchaseOrderApprovedResponse>> Handle(NewPurchaseOrderGetAllApprovedQuery request, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            Func<Task<List<PurchaseOrder>>> getAll = () => QueryRepository.GetAllPurchaseOrderApprovedAsync();
            try
            {
                var List = await _cache.GetOrAddAsync(Cache.GetAllPurchaseOrderApproved, getAll);
                NewPriorPurchaseOrderApprovedResponse responseList = new NewPriorPurchaseOrderApprovedResponse()
                {
                    PurchaseOrders = List.Select(x => x.ToPurchaseOrderResponse()).ToList(),
                };
                return Result<NewPriorPurchaseOrderApprovedResponse>.Success(responseList);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Result<NewPriorPurchaseOrderApprovedResponse>.Fail(message);
        }
    }
}
