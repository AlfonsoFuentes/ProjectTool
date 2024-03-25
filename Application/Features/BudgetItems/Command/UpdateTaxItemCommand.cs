using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Command
{
    public record UpdateTaxItemCommand(UpdateBudgetItemRequest Data) : IRequest<IResult>;
    public class UpdateTaxItemCommandHandler : IRequestHandler<UpdateTaxItemCommand, IResult>
    {
        private IBudgetItemRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public UpdateTaxItemCommandHandler(IAppDbContext appDbContext, IBudgetItemRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(UpdateTaxItemCommand request, CancellationToken cancellationToken)
        {
           
            var row = await Repository.GetBudgetItemWithTaxesById(request.Data.Id);
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
            row.Percentage = request.Data.Percentage;
            if(row.IsMainItemTaxesNoProductive)
            {
                mwo.PercentageAssetNoProductive = row.Percentage;
                await Repository.UpdateMWO(mwo);
            }
            foreach (var taxitem in row.TaxesItems)
            {
                if (!request.Data.SelectedBudgetItemDtos.Any(x => x.SelectedItemId == taxitem.SelectedId))
                {
                    AppDbContext.TaxesItems.Remove(taxitem);
                }
            }
            double sumBudget = 0;
            foreach (var itemdto in request.Data.SelectedBudgetItemDtos)
            {
                sumBudget += itemdto.Budget * row.Percentage / 100.0;
                if (!row.TaxesItems.Any(x => x.SelectedId == itemdto.SelectedItemId))
                {
                    var taxItem = row.AddTaxItem(itemdto.BudgetItemId);
                    await Repository.AddTaxSelectedItem(taxItem);
                }


            }
            row.Budget = sumBudget;
            await Repository.UpdateBudgetItem(row);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            await Repository.UpdateTaxesAndEngineeringContingencyItems(row.MWOId, cancellationToken);
            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} updated succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not updated succesfully!");
        }
        
    }
}
