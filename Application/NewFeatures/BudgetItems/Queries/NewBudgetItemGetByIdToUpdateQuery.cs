namespace Application.NewFeatures.BudgetItems.Queries
{
    public record NewBudgetItemGetByIdToUpdateQuery(Guid BudgetItemId) : IRequest<IResult<NewBudgetItemMWOUpdateRequest>>;

    internal class NewBudgetItemGetByIdToUpdateQueryHandler : IRequestHandler<NewBudgetItemGetByIdToUpdateQuery, IResult<NewBudgetItemMWOUpdateRequest>>
    {
        private IQueryRepository QueryRepository { get; set; }
        private readonly IAppDbContext _cache;
        public NewBudgetItemGetByIdToUpdateQueryHandler(IQueryRepository queryRepository, IAppDbContext cache)
        {
            QueryRepository = queryRepository;
            _cache = cache;
        }

        public async Task<IResult<NewBudgetItemMWOUpdateRequest>> Handle(NewBudgetItemGetByIdToUpdateQuery request, CancellationToken cancellationToken)
        {
            var row = await QueryRepository.GetBudgetItemToUpdateByIdAsync(request.BudgetItemId);
            if (row == null)
            {
                return Result<NewBudgetItemMWOUpdateRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.BudgetItems));
            }
            NewBudgetItemMWOUpdateRequest response= row.ToBudgetItemMWOUpdateRequest();

            return Result<NewBudgetItemMWOUpdateRequest>.Success(response);
        }
    }
}
