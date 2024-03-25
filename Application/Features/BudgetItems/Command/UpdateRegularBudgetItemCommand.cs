using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Command
{
    public record UpdateRegularBudgetItemCommand(UpdateBudgetItemRequest Data) : IRequest<IResult>;
    public class UpdateRegularBudgetItemCommandHandler : IRequestHandler<UpdateRegularBudgetItemCommand, IResult>
    {
        private IBudgetItemRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public UpdateRegularBudgetItemCommandHandler(IAppDbContext appDbContext, IBudgetItemRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(UpdateRegularBudgetItemCommand request, CancellationToken cancellationToken)
        {
           
            var row = await Repository.GetBudgetItemById(request.Data.Id);
           
            if (row == null )
            {
                return Result.Fail($"Not found");
            }
            row.Name = request.Data.Name;
            row.UnitaryCost = request.Data.UnitaryCost;
            row.Budget = request.Data.UnitaryCost * request.Data.Quantity;
            row.Existing = request.Data.Existing;
            row.Quantity = request.Data.Quantity;

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
