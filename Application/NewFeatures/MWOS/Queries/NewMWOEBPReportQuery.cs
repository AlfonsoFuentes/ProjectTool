namespace Application.NewFeatures.MWOS.Queries
{
    public record NewMWOEBPReportQuery(Guid MWOId) : IRequest<IResult<NewEBPReportResponse>>;
    internal class NewMWOEBPReportQueryHandler : IRequestHandler<NewMWOEBPReportQuery, IResult<NewEBPReportResponse>>
    {
        private readonly IQueryRepository QueryRepository;
        private readonly IAppDbContext _cache;
        public NewMWOEBPReportQueryHandler(IQueryRepository queryRepository, IAppDbContext cache)
        {
            QueryRepository = queryRepository;
            _cache = cache;
        }

        public async Task<IResult<NewEBPReportResponse>> Handle(NewMWOEBPReportQuery request, CancellationToken cancellationToken)
        {
            string message = string.Empty;
            
            Func<Task<MWO?>> getbyid = () => QueryRepository.GetMWOByIdWithPurchaseOrderAsync(request.MWOId);
            try
            {

                var mwo = await _cache.GetOrAddAsync($"{Cache.GetMWOPurchaseOrderById}:{request.MWOId}", getbyid);
                if (mwo == null)
                {
                    return Result<NewEBPReportResponse>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.MWO));
                }

                NewEBPReportResponse response = mwo.ToMWOEBPReportResponse();
                return Result<NewEBPReportResponse>.Success(response);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }




            return Result<NewEBPReportResponse>.Fail(message);

        }
    }
}
