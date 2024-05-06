

namespace Application.NewFeatures.MWOS.Queries
{
    public record NewMWOGetAllApprovedQuery : IRequest<IResult<NewMWOApprovedListReponse>>;
    internal class NewMWOGetAllApprovedQueryHandler : IRequestHandler<NewMWOGetAllApprovedQuery, IResult<NewMWOApprovedListReponse>>
    {
        private readonly IQueryRepository QueryRepository;
        private readonly IAppDbContext _cache;
        public NewMWOGetAllApprovedQueryHandler(IQueryRepository queryRepository, IAppDbContext cache)
        {
            QueryRepository = queryRepository;
            _cache = cache;
        }

        public async Task<IResult<NewMWOApprovedListReponse>> Handle(NewMWOGetAllApprovedQuery request, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            Func<Task<List<MWO>>> getAll = () => QueryRepository.GetAllMWOsApprovedAsync();
            try
            {
                var List = await _cache.GetOrAddAsync(Cache.GetAllMWOsApproved, getAll);
                NewMWOApprovedListReponse responseList = new NewMWOApprovedListReponse()
                {
                    MWOsApproved = List.Select(x => x.ToMWOApprovedReponse()).ToList(),
                };
                return Result<NewMWOApprovedListReponse>.Success(responseList);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Result<NewMWOApprovedListReponse>.Fail(message);
        }
    }
}
