using Application.Mappers.BudgetItems;

namespace Application.NewFeatures.BudgetItems.Queries
{
    public record NewBudgetItemGetByIdMWOApprovedQuery(Guid BudgetItemId) : IRequest<IResult<NewBudgetItemToCreatePurchaseOrderResponse>>;
    internal class NewBudgetItemGetByIdMWOApprovedQueryHandler : IRequestHandler<NewBudgetItemGetByIdMWOApprovedQuery, IResult<NewBudgetItemToCreatePurchaseOrderResponse>>
    {
        private IQueryRepository QueryRepository { get; set; }
        private readonly IAppDbContext _cache;
        public NewBudgetItemGetByIdMWOApprovedQueryHandler(IQueryRepository queryRepository, IAppDbContext cache)
        {
            QueryRepository = queryRepository;
            _cache = cache;
        }

        public async Task<IResult<NewBudgetItemToCreatePurchaseOrderResponse>> Handle(NewBudgetItemGetByIdMWOApprovedQuery request, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            Func<Task<BudgetItem?>> getbyid = () => QueryRepository.GetBudgetItemMWOApprovedAsync(request.BudgetItemId);
            try
            {
                var result = await _cache.GetOrAddAsync($"{Cache.GetBudgetItemMWOApproved}:{request.BudgetItemId}", getbyid);
                if (result == null)
                {
                    return Result<NewBudgetItemToCreatePurchaseOrderResponse>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.BudgetItems));
                }
                NewBudgetItemToCreatePurchaseOrderResponse response = result.ToBudgetItemToCreatePurchaseOrder();
                return Result<NewBudgetItemToCreatePurchaseOrderResponse>.Success(response);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            return Result<NewBudgetItemToCreatePurchaseOrderResponse>.Fail(message);



        }
    }
}
