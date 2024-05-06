using Shared.Enums.BudgetItemTypes;
using Shared.Enums.MWOTypes;
using Shared.Models.MWO;

namespace Application.Features.MWOs.Queries
{
    public record GetMWOToUpdateByIdQuery(Guid Id) : IRequest<IResult<UpdateMWORequest>>;
    public class GetMWOToUpdateByIdQueryHandler : IRequestHandler<GetMWOToUpdateByIdQuery, IResult<UpdateMWORequest>>
    {
        private IMWORepository Repository { get; set; }
 
        public GetMWOToUpdateByIdQueryHandler(IMWORepository repository)
        {
            Repository = repository;
        
        }


        public async Task<IResult<UpdateMWORequest>> Handle(GetMWOToUpdateByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await Repository.GetMWOById(request.Id);
            if (result == null) return Result<UpdateMWORequest>.Fail("Not Found");
            var mwo = await Repository.GetMWOWithItemsById(request.Id);
            UpdateMWORequest retorno = new()
            {
                Id = mwo.Id,
                Name = mwo.Name,
                IsAssetProductive = mwo.IsAssetProductive,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo.PercentageContingency,
                PercentageEngineering = mwo.PercentageCapitalizedSalary,
                Type=MWOTypeEnum.GetType(mwo.Type),
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
            
            return Result<UpdateMWORequest>.Success(retorno);

        }
    }

}
