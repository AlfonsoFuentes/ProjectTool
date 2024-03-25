using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;

namespace Application.Features.BudgetItems.Command
{
    public record UpdateBudgetItemMinimalCommand(UpdateBudgetItemMinimalRequest Data) : IRequest<IResult>;
    public class UpdateBudgetItemMinimalCommandHandler : IRequestHandler<UpdateBudgetItemMinimalCommand, IResult>
    {
        private IBudgetItemRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public UpdateBudgetItemMinimalCommandHandler(IAppDbContext appDbContext, IBudgetItemRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(UpdateBudgetItemMinimalCommand request, CancellationToken cancellationToken)
        {
           
            var row = await Repository.GetBudgetItemById(request.Data.Id);
            var mwo = await Repository.GetMWOById(row.MWOId);
            if (row == null)
            {
                return Result.Fail($"Budget Item not found");
            }
           
            if (mwo == null)
            {
                return Result.Fail($"MWO not found");
            }
            row.Name = request.Data.Name;
            row.UnitaryCost = request.Data.UnitaryCost;
            row.Budget = request.Data.Budget;
            row.Percentage = request.Data.Percentage;
            row.Quantity = request.Data.Quantity;

            await UpdatePercentageMWO(row, mwo);
            await Repository.UpdateBudgetItem(row);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);


            await Repository.UpdateTaxesAndEngineeringContingencyItems(row.MWOId, cancellationToken);
            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} updated succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not updated succesfully!");
        }
        async Task UpdatePercentageMWO(BudgetItem budgetitem, MWO mwo)
        {
            if (budgetitem.Type == BudgetItemTypeEnum.Taxes.Id && budgetitem.IsMainItemTaxesNoProductive)
            {
                mwo.PercentageAssetNoProductive = budgetitem.Percentage;
                await Repository.UpdateMWO(mwo);
            }
            if (budgetitem.Type == BudgetItemTypeEnum.Engineering.Id)
            {
                mwo.PercentageEngineering = budgetitem.Percentage;
                await Repository.UpdateMWO(mwo);
            }
            if (budgetitem.Type == BudgetItemTypeEnum.Contingency.Id)
            {
                mwo.PercentageContingency = budgetitem.Percentage;
                await Repository.UpdateMWO(mwo);
            }
        }
    }
}
