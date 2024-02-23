using Application.Features.BudgetItems.Validators;
using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Command
{
    public record CreateTaxItemCommand(CreateBudgetItemRequest Data) : IRequest<IResult>;
    public class CreateTaxItemCommandHandler : IRequestHandler<CreateTaxItemCommand, IResult>
    {
        private IBudgetItemRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public CreateTaxItemCommandHandler(IAppDbContext appDbContext, IBudgetItemRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(CreateTaxItemCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateBudgetItemValidator(Repository);
            var validatorresult = await validator.ValidateAsync(request.Data);
            if (!validatorresult.IsValid)
            {
                return Result.Fail(validatorresult.Errors.Select(x => x.ErrorMessage).ToList());
            }
            var mwo = await Repository.GetMWOWithItemsById(request.Data.MWOId);

            if (mwo == null) return Result.Fail("MWO not found!");

            var row = mwo.AddBudgetItem(request.Data.Type.Id);
            row.Name = request.Data.Name;
            row.Percentage = request.Data.Percentage;
            double sumBudget = 0;
            foreach (var itemdto in request.Data.BudgetItemDtos)
            {
                sumBudget += itemdto.Budget * row.Percentage / 100.0;
                var taxItem = row.AddTaxItem(itemdto.Id);
                await Repository.AddTaxSelectedItem(taxItem);
            }
            row.Budget = sumBudget;
            await Repository.AddBudgetItem(row);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            await Repository.UpdateTaxesAndEngineeringContingencyItems(row.MWOId, cancellationToken);
            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} created succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not created succesfully!");
        }
        
    }
}
