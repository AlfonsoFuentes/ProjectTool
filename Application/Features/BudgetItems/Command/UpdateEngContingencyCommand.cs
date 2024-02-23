using Application.Features.BudgetItems.Validators;
using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var validator = new UpdateBudgetItemValidator(Repository);
            var validatorresult = await validator.ValidateAsync(request.Data);
            if (!validatorresult.IsValid)
            {
                return Result.Fail(validatorresult.Errors.Select(x => x.ErrorMessage).ToList());
            }
            var row = await Repository.GetBudgetItemById(request.Data.Id);
           
            if (row == null )
            {
                return Result.Fail($"Not found");
            }

            row.Name = request.Data.Name;
            row.Percentage = request.Data.Percentage;
            row.Budget = request.Data.Percentage == 0 ? request.Data.Budget : 0;
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
