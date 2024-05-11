﻿using Shared.Enums.CostCenter;
using Shared.Enums.MWOStatus;
using Shared.Enums.MWOTypes;
using Shared.Models.MWO;
using System.Diagnostics;

namespace Application.Features.MWOs.Queries
{
    public record GetAllMWOCreatedQuery() : IRequest<IQueryable<MWOCreatedResponse>>;
    public class GetAllMWOCreatedQueryHandler : IRequestHandler<GetAllMWOCreatedQuery, IQueryable<MWOCreatedResponse>>
    {
        private IMWORepository Repository { get; set; }


        public GetAllMWOCreatedQueryHandler(IMWORepository repository)
        {
            Repository = repository;

        }


        public async Task<IQueryable<MWOCreatedResponse>> Handle(GetAllMWOCreatedQuery request, CancellationToken cancellationToken)
        {
            Stopwatch sw = Stopwatch.StartNew();

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
                MWOStatus = MWOStatusEnum.GetType(mwo.Status),
                PercentageTaxForAlterations = mwo.PercentageTaxForAlterations,
                HasExpenses = mwo.HasExpenses,


            });
            
            var elapse2 = sw.ElapsedMilliseconds;
            return result.AsQueryable();

        }
       
    }
}