using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;

namespace Application.Features.BudgetItems.Command
{
    public record UpdateEngContingencyCommand(UpdateBudgetItemRequest Data) : IRequest<IResult>;
    public class UpdateEngContingencyCommandHandler : IRequestHandler<UpdateEngContingencyCommand, IResult>
    {
        private IBudgetItemRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public UpdateEngContingencyCommandHandler(IAppDbContext appDbContext, IBudgetItemRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(UpdateEngContingencyCommand request, CancellationToken cancellationToken)
        {
           
            var row = await Repository.GetBudgetItemById(request.Data.Id);
            var mwo = await Repository.GetMWOById(row.MWOId);
            if (row == null )
            {
                return Result.Fail($"Not found");
            }
            if (row == null)
            {
                return Result.Fail($"Budget Item not found");
            }
            row.Name = request.Data.Name;
            row.Percentage = request.Data.Percentage;
            row.UnitaryCost = request.Data.UnitaryCost;
            row.Quantity = request.Data.Quantity;
            row.Budget = request.Data.Percentage == 0 ? request.Data.Budget : 0;
            if(row.Type==BudgetItemTypeEnum.Engineering.Id)
            {
                mwo.PercentageEngineering = row.Percentage;
                await Repository.UpdateMWO(mwo);
            }
            if (row.Type == BudgetItemTypeEnum.Contingency.Id)
            {
                mwo.PercentageContingency = row.Percentage;
                await Repository.UpdateMWO(mwo);
            }
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
           
            await AppDbContext.SaveChangesAsync(cancellationToken);
            await Repository.UpdateTaxesAndEngineeringContingencyItems(row.MWOId, cancellationToken);
            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} updated succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not updated succesfully!");
        }
        
    }
}
