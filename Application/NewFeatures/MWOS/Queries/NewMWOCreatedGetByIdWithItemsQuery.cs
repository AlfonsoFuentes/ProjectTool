

namespace Application.NewFeatures.MWOS.Queries
{
    public record NewMWOCreatedGetByIdWithItemsQuery(Guid MWOId) : IRequest<IResult<NewMWOCreatedWithItemsResponse>>;
    internal class NewMWOCreatedGetByIdWithItemsQueryQueryHandler : IRequestHandler<NewMWOCreatedGetByIdWithItemsQuery, IResult<NewMWOCreatedWithItemsResponse>>
    {
        private readonly IQueryRepository QueryRepository;
        private readonly IAppDbContext _cache;
        public NewMWOCreatedGetByIdWithItemsQueryQueryHandler(IQueryRepository queryRepository, IAppDbContext cache)
        {
            QueryRepository = queryRepository;
            _cache = cache;
        }

        public async Task<IResult<NewMWOCreatedWithItemsResponse>> Handle(NewMWOCreatedGetByIdWithItemsQuery request, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            Func<Task<MWO?>> getbyid = () => QueryRepository.GetMWOByIdCreatedAsync(request.MWOId);
            try
            {
                var mwo = await _cache.GetOrAddAsync($"{Cache.GetMWOByCreated}:{request.MWOId}", getbyid);
                if (mwo == null)
                {
                    return Result<NewMWOCreatedWithItemsResponse>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.MWO));
                }

                NewMWOCreatedWithItemsResponse response = mwo.ToMWOWithItemsCreatedResponse();
                return Result<NewMWOCreatedWithItemsResponse>.Success(response);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }




            return Result<NewMWOCreatedWithItemsResponse>.Fail(message);

        }
    }
}
