using Application.Interfaces;
using MediatR;
using Shared.Models.CostCenter;
using Shared.Models.MWO;
using Shared.Models.MWOStatus;
using Shared.Models.MWOTypes;
using System.Diagnostics;

namespace Application.Features.MWOs.Queries
{
    public record GetAllMWOClosedQuery() : IRequest<IQueryable<MWOClosedResponse>>;
    public class GetAllMWOClosedQueryHandler : IRequestHandler<GetAllMWOClosedQuery, IQueryable<MWOClosedResponse>>
    {
        private IMWORepository Repository { get; set; }


        public GetAllMWOClosedQueryHandler(IMWORepository repository)
        {
            Repository = repository;

        }


        public async Task<IQueryable<MWOClosedResponse>> Handle(GetAllMWOClosedQuery request, CancellationToken cancellationToken)
        {
            Stopwatch sw = Stopwatch.StartNew();

            var query = await Repository.GetMWOClosedList();
            var result = query.Select(mwo => new MWOClosedResponse
            {
                Id = mwo.Id,
                Name = mwo.Name,
                IsAssetProductive = mwo.IsAssetProductive,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo.PercentageContingency,
                PercentageEngineering = mwo.PercentageEngineering,
                MWOType = MWOTypeEnum.GetType(mwo.Type),

                CECName = $"CEC0000{mwo.MWONumber}",
                CostCenter = CostCenterEnum.GetName(mwo.CostCenter),
                CreatedBy = mwo.CreatedByUserName,
                CreatedOn = mwo.CreatedDate.ToString("d"),

                MWOStatus = MWOStatusEnum.GetType(mwo.Status),
                PercentageTaxForAlterations = mwo.PercentageTaxForAlterations,
                HasExpenses = mwo.HasExpenses,
                Capital = new()
                {
                    BudgetUSD = mwo.CapitalUSD,
                    ActualUSD = mwo.CapitalActualUSD,
                    ApprovedUSD = mwo.CapitalApprovedUSD,
                    AssignedUSD = mwo.CapitalAssignedUSD,
                    PotentialCommitmentUSD = mwo.CapitalPotentialCommitmentUSD,
                },
                Expenses = new()
                {
                    BudgetUSD = mwo.ExpensesUSD,
                    ActualUSD = mwo.ExpensesActualUSD,
                    ApprovedUSD = mwo.ExpensesApprovedUSD,
                    PotentialCommitmentUSD = mwo.ExpensesPotentialCommitmentUSD,
                    AssignedUSD = mwo.ExpensesAssignedUSD,

                },


            });
            sw.Stop();
            var elapse2 = sw.ElapsedMilliseconds;
            return result.AsQueryable();

        }

    }
}
