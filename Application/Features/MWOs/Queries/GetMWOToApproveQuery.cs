using Shared.Enums.BudgetItemTypes;
using Shared.Enums.CostCenter;
using Shared.Enums.MWOTypes;
using Shared.Models.MWO;

namespace Application.Features.MWOs.Queries
{
    public record GetMWOToApproveQuery(Guid MWOId) : IRequest<IResult<ApproveMWORequest>>;

    public class GetMWOToApproveQueryHandler : IRequestHandler<GetMWOToApproveQuery, IResult<ApproveMWORequest>>
    {
        private IMWORepository Repository { get; set; }

        public GetMWOToApproveQueryHandler(IMWORepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<ApproveMWORequest>> Handle(GetMWOToApproveQuery request, CancellationToken cancellationToken)
        {

            var mwo = await Repository.GetMWOWithItemsById(request.MWOId);
            ApproveMWORequest approveMWORequest = new()
            {
                Id = mwo.Id,
                Name = mwo.Name,
                Type=MWOTypeEnum.GetType(mwo.Type),
                MWONumber=mwo.MWONumber,
                CostCenter=CostCenterEnum.GetType(mwo.CostCenter),
                IsAssetProductive = mwo.IsAssetProductive,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo.PercentageContingency,
                PercentageEngineering = mwo.PercentageCapitalizedSalary,
                PercentageTaxForAlterations = mwo.PercentageTaxForAlterations,
                BudgetItems = mwo.BudgetItems.Select(x =>
                new BudgetItemResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Nomenclatore = $"{BudgetItemTypeEnum.GetLetter(x.Type)}{x.Order}",
                    Type = BudgetItemTypeEnum.GetType(x.Type),
                    Budget = x.Budget,
                    Percentage = x.Percentage,
                    IsNotAbleToEditDelete = x.IsNotAbleToEditDelete,
                    IsMainItemTaxesNoProductive = x.IsMainItemTaxesNoProductive,
                }
                 ).ToList(),

            };

            return Result<ApproveMWORequest>.Success(approveMWORequest);
        }
    }

}
