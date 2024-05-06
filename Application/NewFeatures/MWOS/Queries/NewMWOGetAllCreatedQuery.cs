

namespace Application.NewFeatures.MWOS.Queries
{
    public record NewMWOGetAllCreatedQuery : IRequest<IResult<NewMWOCreatedListResponse>>;

    internal class NewMWOGetAllCreatedQueryHandler : IRequestHandler<NewMWOGetAllCreatedQuery, IResult<NewMWOCreatedListResponse>>
    {
        private readonly IQueryRepository QueryRepository;
        private readonly IAppDbContext _cache;
        public NewMWOGetAllCreatedQueryHandler(IQueryRepository queryRepository, IAppDbContext cache)
        {
            QueryRepository = queryRepository;
            _cache = cache;
        }

        public async Task<IResult<NewMWOCreatedListResponse>> Handle(NewMWOGetAllCreatedQuery request, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            Func<Task<List<MWO>>> getAll = () => QueryRepository.GetAllMWOsCreatedAsync();
            try
            {
                var List = await _cache.GetOrAddAsync(Cache.GetAllMWOsCreated, getAll);
                NewMWOCreatedListResponse responseList = new NewMWOCreatedListResponse()
                {
                    MWOsCreated = List.Select(x => x.ToMWOCreatedResponse()).ToList(),
                };
                return Result<NewMWOCreatedListResponse>.Success(responseList);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Result<NewMWOCreatedListResponse>.Fail(message);
        }
    }
}
