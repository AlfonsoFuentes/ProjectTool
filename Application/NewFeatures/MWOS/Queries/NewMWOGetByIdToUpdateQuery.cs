namespace Application.NewFeatures.MWOS.Queries
{
    public record NewMWOGetByIdToUpdateQuery(Guid MWOId) : IRequest<IResult<NewMWOUpdateRequest>>;

    internal class NewMWOGetByIdToUpdateQueryHandler : IRequestHandler<NewMWOGetByIdToUpdateQuery, IResult<NewMWOUpdateRequest>>
    {
        private IQueryRepository Repository { get; set; }
        private readonly IAppDbContext _cache;
        public NewMWOGetByIdToUpdateQueryHandler(IQueryRepository repository, IAppDbContext cache)
        {
            Repository = repository;
            _cache = cache;
        }

        public async Task<IResult<NewMWOUpdateRequest>> Handle(NewMWOGetByIdToUpdateQuery request, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            Func<Task<MWO?>> getbyid = () => Repository.GetMWOByIdCreatedAsync(request.MWOId);
            try
            {

                var mwo = await _cache.GetOrAddAsync($"{Cache.GetMWOByCreated}:{request.MWOId}", getbyid);
                if (mwo == null)
                {
                    return Result<NewMWOUpdateRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.MWO));
                }

                NewMWOUpdateRequest response = mwo.ToMWOUpdateRequest();
                return Result<NewMWOUpdateRequest>.Success(response);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Result<NewMWOUpdateRequest>.Fail(message);
           

        }
    }
}
