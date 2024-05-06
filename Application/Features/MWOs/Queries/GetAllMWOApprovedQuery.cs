﻿using Shared.Enums.CostCenter;
using Shared.Enums.MWOStatus;
using Shared.Enums.MWOTypes;
using Shared.Models.MWO;
using System.Diagnostics;

namespace Application.Features.MWOs.Queries
{
    public record GetAllMWOApprovedQuery() : IRequest<IQueryable<MWOApprovedResponse>>;
    public class GetAllMWOApprovedQueryHandler : IRequestHandler<GetAllMWOApprovedQuery, IQueryable<MWOApprovedResponse>>
    {
        private IMWORepository Repository { get; set; }


        public GetAllMWOApprovedQueryHandler(IMWORepository repository)
        {
            Repository = repository;

        }


        public async Task<IQueryable<MWOApprovedResponse>> Handle(GetAllMWOApprovedQuery request, CancellationToken cancellationToken)
        {
            Stopwatch sw = Stopwatch.StartNew();

            var query = await Repository.GetMWOApprovedList();
            var result = query.Select(mwo => new MWOApprovedResponse
            {
                Id = mwo.Id,
                Name = mwo.Name,
                IsAssetProductive = mwo.IsAssetProductive,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo.PercentageContingency,
                PercentageEngineering = mwo.PercentageCapitalizedSalary,
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
