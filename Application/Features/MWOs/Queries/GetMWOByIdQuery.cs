using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;
using Shared.Models.MWOStatus;
using Shared.Models.MWOTypes;

namespace Application.Features.MWOs.Queries
{
    public record GetMWOByIdQuery(Guid Id) : IRequest<IResult<MWOResponse>>;
    public class GetMWOByIdQueryHandler : IRequestHandler<GetMWOByIdQuery, IResult<MWOResponse>>
    {
        private IMWORepository Repository { get; set; }

        public GetMWOByIdQueryHandler(IMWORepository repository)
        {
            Repository = repository;

        }


        public async Task<IResult<MWOResponse>> Handle(GetMWOByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await Repository.GetMWOById(request.Id);
            if (result == null) return Result<MWOResponse>.Fail("Not Found");
            var mwo = await Repository.GetMWOWithItemsById(request.Id);
            MWOResponse retorno = new()
            {
                Id = mwo.Id,
                Name = mwo.Name,
                IsAssetProductive = mwo.IsAssetProductive,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo.PercentageContingency,
                PercentageEngineering = mwo.PercentageEngineering,
                MWOType = MWOTypeEnum.GetType(mwo.Type),
                Capital = mwo.Capital,
                Expenses = mwo.Expenses,
                PotencialCapital = mwo.PotentialCommitmentCapital,
                PotencialExpenses = mwo.PotentialCommitmentExpenses,
                ActualCapital = mwo.ActualCapital,
                ActualExpenses = mwo.ActualExpenses,
                CommitmentCapital = mwo.CommitmentCapital,
                CommitmentExpenses = mwo.CommitmentExpenses,
                Status=MWOStatusEnum.GetType(mwo.Status),

            };

            return Result<MWOResponse>.Success(retorno);

        }
    }
}
