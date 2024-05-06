using Shared.Enums.CostCenter;
using Shared.Enums.MWOStatus;
using Shared.Enums.MWOTypes;
using Shared.Models.MWO;

namespace Application.Features.MWOs.Queries
{
    public record GetMWOCreatedByIdQuery(Guid Id) : IRequest<IResult<MWOCreatedResponse>>;
    public class GetMWOByIdQueryHandler : IRequestHandler<GetMWOCreatedByIdQuery, IResult<MWOCreatedResponse>>
    {
        private IMWORepository Repository { get; set; }

        public GetMWOByIdQueryHandler(IMWORepository repository)
        {
            Repository = repository;

        }


        public async Task<IResult<MWOCreatedResponse>> Handle(GetMWOCreatedByIdQuery request, CancellationToken cancellationToken)
        {
            var mwo = await Repository.GetMWOWithItemsById(request.Id);
            if (mwo == null) return Result<MWOCreatedResponse>.Fail("Not Found");

            MWOCreatedResponse retorno = new()
            {
                Id = mwo.Id,
                Name = mwo.Name,
                IsAssetProductive = mwo.IsAssetProductive,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo.PercentageContingency,
                PercentageEngineering = mwo.PercentageCapitalizedSalary,
                MWOType = MWOTypeEnum.GetType(mwo.Type),
                CapitalUSD = mwo.CapitalUSD,
                CECName = $"CEC0000{mwo.MWONumber}",
                CostCenter = CostCenterEnum.GetName(mwo.CostCenter),
                CreatedBy = mwo.CreatedByUserName,
                CreatedOn = mwo.CreatedDate.ToString("d"),
                ExpensesUSD = mwo.ExpensesUSD,
                MWOStatus = MWOStatusEnum.GetType(mwo.Status),
                PercentageTaxForAlterations = mwo.PercentageTaxForAlterations,



            };

            return Result<MWOCreatedResponse>.Success(retorno);

        }
    }
}
