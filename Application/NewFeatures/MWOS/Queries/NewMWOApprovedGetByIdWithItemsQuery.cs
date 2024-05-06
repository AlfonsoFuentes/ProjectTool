

namespace Application.NewFeatures.MWOS.Queries
{
    public record NewMWOApprovedGetByIdWithItemsQuery(Guid MWOId) : IRequest<IResult<NewMWOApprovedReponse>>;
    internal class NewMWOApprovedGetByIdWithItemsQueryHandler : IRequestHandler<NewMWOApprovedGetByIdWithItemsQuery, IResult<NewMWOApprovedReponse>>
    {
        private readonly IQueryRepository QueryRepository;
        private readonly IAppDbContext _cache;
        public NewMWOApprovedGetByIdWithItemsQueryHandler(IQueryRepository queryRepository, IAppDbContext cache)
        {
            QueryRepository = queryRepository;
            _cache = cache;
        }

        public async Task<IResult<NewMWOApprovedReponse>> Handle(NewMWOApprovedGetByIdWithItemsQuery request, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            Func<Task<MWO?>> getbyid = () => QueryRepository.GetMWOByIdApprovedAsync(request.MWOId);
            try
            {
                var testCache = $"{Cache.GetMWOByApproved}:{request.MWOId}";
                var mwo = await _cache.GetOrAddAsync(testCache, getbyid);
                if (mwo == null)
                {
                    return Result<NewMWOApprovedReponse>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.MWO));
                }

                NewMWOApprovedReponse response = mwo.ToMWOApprovedReponse();
                return Result<NewMWOApprovedReponse>.Success(response);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }




            return Result<NewMWOApprovedReponse>.Fail(message);

        }
    }
}
