using Shared.Enums.CostCenter;
using Shared.Enums.MWOStatus;
using Shared.Enums.MWOTypes;
using Shared.Models.MWO;
using System.Diagnostics;

namespace Application.Features.MWOs.Queries
{
    public record GetAllMWOResponseQuery : IRequest<IResult<MWOResponseList>>;
    public class GetAllMWOResponseQueryHandler : IRequestHandler<GetAllMWOResponseQuery, IResult<MWOResponseList>>
    {
        private IMWORepository Repository { get; set; }


        public GetAllMWOResponseQueryHandler(IMWORepository repository)
        {
            Repository = repository;

        }


        public async Task<IResult<MWOResponseList>> Handle(GetAllMWOResponseQuery request, CancellationToken cancellationToken)
        {
            Stopwatch sw = Stopwatch.StartNew();

           
            MWOResponseList response = new();
            response.MWOsCreated = await GetCreated();
            response.MWOsApproved = await GetApproved();
            response.MWOsClosed = await GetClosed();
            sw.Stop();
            var elapse2 = sw.ElapsedMilliseconds;
            return Result<MWOResponseList>.Success(response);

        }
        async Task<IEnumerable<MWOCreatedResponse>> GetCreated()
        {
            var query = await Repository.GetMWOCreatedList();
            var result = query.Select(mwo => new MWOCreatedResponse
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
                MWOStatus = MWOStatusEnum.Created,
                PercentageTaxForAlterations = mwo.PercentageTaxForAlterations,
                HasExpenses = mwo.HasExpenses,


            });
            return result;
        }
        async Task<IEnumerable<MWOApprovedResponse>> GetApproved()
        {
            var mwoList = await Repository.GetMWOApprovedList();
            var result = mwoList.Select(mwo => new MWOApprovedResponse
            {
                Id = mwo.Id,
                Name = mwo.Name,
                CreatedBy = mwo.CreatedByUserName,
                CreatedOn = mwo.CreatedDate.ToString(),
                CostCenter = CostCenterEnum.GetName(mwo.CostCenter),
                IsAssetProductive = mwo.IsAssetProductive,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo.PercentageContingency,
                PercentageEngineering = mwo.PercentageCapitalizedSalary,
                MWOType = MWOTypeEnum.GetType(mwo.Type),
                CECName = $"CEC0000{mwo.MWONumber}",
                MWOStatus = MWOStatusEnum.Approved,
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
            return result;
        }
        async Task<IEnumerable<MWOClosedResponse>> GetClosed()
        {
            var mwoList = await Repository.GetMWOClosedList();
            var result = mwoList.Select(mwo => new MWOClosedResponse
            {
                Id = mwo.Id,
                Name = mwo.Name,
                CreatedBy = mwo.CreatedByUserName,
                CreatedOn = mwo.CreatedDate.ToString(),
                CostCenter = CostCenterEnum.GetName(mwo.CostCenter),
                IsAssetProductive = mwo.IsAssetProductive,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo.PercentageContingency,
                PercentageEngineering = mwo.PercentageCapitalizedSalary,
                MWOType = MWOTypeEnum.GetType(mwo.Type),
                CECName = $"CEC0000{mwo.MWONumber}",
                MWOStatus = MWOStatusEnum.Approved,
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
            return result;
        }
    }
}
