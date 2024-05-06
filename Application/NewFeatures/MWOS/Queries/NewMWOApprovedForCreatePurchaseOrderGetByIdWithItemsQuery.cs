

namespace Application.NewFeatures.MWOS.Queries
{
    public record NewMWOApprovedForCreatePurchaseOrderGetByIdQuery(Guid MWOId) : IRequest<IResult<NewMWOApprovedForCreatePurchaseOrderReponse>>;
    internal class NewMWOApprovedForCreatePurchaseOrderGetByIdQueryHandler : IRequestHandler<NewMWOApprovedForCreatePurchaseOrderGetByIdQuery, IResult<NewMWOApprovedForCreatePurchaseOrderReponse>>
    {
        private readonly IQueryRepository QueryRepository;
        private readonly IAppDbContext _cache;
        public NewMWOApprovedForCreatePurchaseOrderGetByIdQueryHandler(IQueryRepository queryRepository, IAppDbContext cache)
        {
            QueryRepository = queryRepository;
            _cache = cache;
        }

        public async Task<IResult<NewMWOApprovedForCreatePurchaseOrderReponse>> Handle(NewMWOApprovedForCreatePurchaseOrderGetByIdQuery request, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            Func<Task<MWO?>> getbyid = () => QueryRepository.GetMWOByIdApprovedAsync(request.MWOId);
            try
            {
                var mwo = await _cache.GetOrAddAsync($"{Cache.GetMWOByApproved}:{request.MWOId}", getbyid);
                if (mwo == null)
                {
                    return Result<NewMWOApprovedForCreatePurchaseOrderReponse>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.MWO));
                }

                NewMWOApprovedForCreatePurchaseOrderReponse response = mwo.ToMWOApprovedForCreatePurchaseOrderReponse();
                return Result<NewMWOApprovedForCreatePurchaseOrderReponse>.Success(response);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }




            return Result<NewMWOApprovedForCreatePurchaseOrderReponse>.Fail(message);

        }
    }
}
