using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
using Shared.Models.SapAdjust;

namespace Application.Features.SapAdjusts.Queries
{
    public record GetSapAdjustByMWOIdQuery(Guid MWOId) : IRequest<IResult<SapAdjustResponseList>>;
    internal class GetSapAdjustByMWOIdQueryHandler : IRequestHandler<GetSapAdjustByMWOIdQuery, IResult<SapAdjustResponseList>>
    {
        private ISapAdjustRepository Repository { get; set; }
   
        public GetSapAdjustByMWOIdQueryHandler(ISapAdjustRepository repository)
        {
            Repository = repository;
  
        }

        public async Task<IResult<SapAdjustResponseList>> Handle(GetSapAdjustByMWOIdQuery request, CancellationToken cancellationToken)
        {
          
            var query = await Repository.GetSapAdjustsByMWOId(request.MWOId);
          
            SapAdjustResponseList response = new()
            {
                MWOName = query.Name,
                MWOCECName = $"CEC0000{query.MWONumber}",
                MWOApprovedDate=query.ApprovedDate.Date,
                Adjustments = query.SapAdjusts.OrderBy(x=>x.Date).Select(x => new SapAdjustResponse()
                {
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
                    BudgetCapital = query.BudgetItems.Where(x => x.Type != BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget),

                }),
            };
          
 
          
            

            return Result<SapAdjustResponseList>.Success(response);
        }
}
}
