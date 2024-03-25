using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;
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

                }
                 ).ToList(),

            };

            return Result<MWOResponse>.Success(retorno);

        }
    }
}
