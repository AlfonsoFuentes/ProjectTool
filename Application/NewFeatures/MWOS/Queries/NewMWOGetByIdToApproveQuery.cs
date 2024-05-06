using Application.Caches;
using Application.Interfaces;

namespace Application.NewFeatures.MWOS.Queries
{
    public record NewMWOGetByIdToApproveQuery(Guid MWOId) : IRequest<IResult<NewMWOApproveRequest>>;
    internal class NewMWOGetByIdToApproveQueryHandler : IRequestHandler<NewMWOGetByIdToApproveQuery, IResult<NewMWOApproveRequest>>
    {
        private IQueryRepository Repository { get; set; }
        private readonly IAppDbContext _cache;
        public NewMWOGetByIdToApproveQueryHandler(IQueryRepository repository, IAppDbContext cache)
        {
            Repository = repository;
            _cache = cache;
        }

        public async Task<IResult<NewMWOApproveRequest>> Handle(NewMWOGetByIdToApproveQuery request, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            Func<Task<MWO?>> getbyid = () => Repository.GetMWOByIdCreatedAsync(request.MWOId);
            try
            {
               
                var mwo = await _cache.GetOrAddAsync($"{Cache.GetMWOByApproved}:{request.MWOId}", getbyid);
                if (mwo == null)
                {
                    return Result<NewMWOApproveRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.MWO));
                }

                NewMWOApproveRequest response = mwo.ToMWOApproveRequest();
                return Result<NewMWOApproveRequest>.Success(response);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            
            return Result<NewMWOApproveRequest>.Fail(message);

        }
    }
}
