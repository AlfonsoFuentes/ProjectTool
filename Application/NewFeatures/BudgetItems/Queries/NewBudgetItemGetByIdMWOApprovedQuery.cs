namespace Application.NewFeatures.BudgetItems.Queries
{
    public record NewBudgetItemGetByIdMWOApprovedQuery(Guid BudgetItemId) : IRequest<IResult<NewBudgetItemMWOApprovedResponse>>;
    internal class NewBudgetItemGetByIdMWOApprovedQueryHandler : IRequestHandler<NewBudgetItemGetByIdMWOApprovedQuery, IResult<NewBudgetItemMWOApprovedResponse>>
    {
        private IQueryRepository QueryRepository { get; set; }
        private readonly IAppDbContext _cache;
        public NewBudgetItemGetByIdMWOApprovedQueryHandler(IQueryRepository queryRepository, IAppDbContext cache)
        {
            QueryRepository = queryRepository;
            _cache = cache;
        }

        public async Task<IResult<NewBudgetItemMWOApprovedResponse>> Handle(NewBudgetItemGetByIdMWOApprovedQuery request, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            Func<Task<BudgetItem?>> getbyid = () => QueryRepository.GetBudgetItemMWOApprovedAsync(request.BudgetItemId);
            try
            {
                var result = await _cache.GetOrAddAsync($"{Cache.GetBudgetItemMWOApproved}:{request.BudgetItemId}", getbyid);
                if (result == null)
                {
                    return Result<NewBudgetItemMWOApprovedResponse>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.BudgetItems));
                }
                NewBudgetItemMWOApprovedResponse response = result.ToBudgetItemMWOApproved();
                return Result<NewBudgetItemMWOApprovedResponse>.Success(response);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            return Result<NewBudgetItemMWOApprovedResponse>.Fail(message);



        }
    }
}
