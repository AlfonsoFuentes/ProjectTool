using Application.Caches;

namespace Application.Features.SapAdjusts.Queries
{
    public record GetSapAdjustByMWOIdQuery(Guid MWOId) : IRequest<IResult<SapAdjustResponseList>>;
    internal class GetSapAdjustByMWOIdQueryHandler : IRequestHandler<GetSapAdjustByMWOIdQuery, IResult<SapAdjustResponseList>>
    {
        private IQueryRepository Repository { get; set; }
        private readonly IAppDbContext _cache;
        public GetSapAdjustByMWOIdQueryHandler(IQueryRepository repository, IAppDbContext cache)
        {
            Repository = repository;
            _cache = cache;
        }

        public async Task<IResult<SapAdjustResponseList>> Handle(GetSapAdjustByMWOIdQuery request, CancellationToken cancellationToken)
        {
            Func<Task<MWO>> getAllSap = () => Repository.GetSapAdjustsByMWOId(request.MWOId);
            try
            {
                var query = await _cache.GetOrAddAsync($"{Cache.GetSapAdjust}:{request.MWOId}", getAllSap);
                SapAdjustResponseList response = new()
                {
                    MWOName = query.Name,
                    MWOCECName = $"CEC0000{query.MWONumber}",
                    MWOApprovedDate = query.ApprovedDate.Date,
                    Adjustments = query.SapAdjusts.OrderBy(x => x.Date).Select(x => new SapAdjustResponse()
                    {
                        CECName = $"CEC0000{query.MWONumber}",
                        ActualSap = x.ActualSap,
                        ActualSoftware = x.ActualSoftware,
                        CommitmentSap = x.CommitmentSap,
                        CommitmentSoftware = x.CommitmentSoftware,
                        Date = x.Date,
                        ImageData = x.ImageDataFile,
                        ImageTitle = x.ImageTitle,
                        Justification = x.Justification,
                        PotencialSap = x.PotencialSap,
                        PotencialSoftware = x.PotencialSoftware,
                        SapAdjustId = x.Id,
                        MWOId=x.MWOId,
                        BudgetCapital = query.BudgetItems.Where(x => x.Type != BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget),

                    }),
                };
                return Result<SapAdjustResponseList>.Success(response);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }


           
            return Result<SapAdjustResponseList>.Fail();
        }
}
}
