using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Commons.UserManagement;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;
using Shared.Models.MWOTypes;
using System.Linq.Expressions;

namespace Application.Features.MWOs.Queries
{
    public record GetByIdMWOQuery(Guid Id) : IRequest<IResult<UpdateMWORequest>>;
    public class GetByIdMWOQueryHandler : IRequestHandler<GetByIdMWOQuery, IResult<UpdateMWORequest>>
    {
        private IMWORepository Repository { get; set; }
 
        public GetByIdMWOQueryHandler(IMWORepository repository)
        {
            Repository = repository;
        
        }


        public async Task<IResult<UpdateMWORequest>> Handle(GetByIdMWOQuery request, CancellationToken cancellationToken)
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
                PercentageEngineering = mwo.PercentageEngineering,
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
