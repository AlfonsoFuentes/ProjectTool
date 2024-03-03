using Application.Features.BudgetItems.Validators;
using Application.Interfaces;
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
            var validator = new UpdateBudgetItemMinimalValidator(Repository);
            var validatorresult = await validator.ValidateAsync(request.Data);
            if (!validatorresult.IsValid)
            {
                return Result.Fail(validatorresult.Errors.Select(x => x.ErrorMessage).ToList());
            }
            var row = await Repository.GetBudgetItemById(request.Data.Id);

            if (row == null)
            {
                return Result.Fail($"Not found");
            }
            var mwo = await Repository.GetMWOById(row.MWOId);
            row.Name = request.Data.Name;
            row.UnitaryCost = request.Data.UnitaryCost;
            row.Budget = request.Data.Budget;
            row.Percentage = request.Data.Percentage;
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
