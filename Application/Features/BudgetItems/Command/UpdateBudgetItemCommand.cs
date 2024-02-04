using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Command
{
    public record UpdateBudgetItemCommand(UpdateBudgetItemRequest Data) : IRequest<IResult>;
    public class UpdateBudgetItemCommandHandler:IRequestHandler<UpdateBudgetItemCommand,IResult>
    {
        private IBudgetItemRepository Repository { get; set; }
        private IAppDbContext Context { get; set; }
        public UpdateBudgetItemCommandHandler(IBudgetItemRepository repository,IAppDbContext context)
        {
            Repository = repository;
            Context = context;
            
        }

        public async Task<IResult> Handle(UpdateBudgetItemCommand request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetBudgetItemById(request.Data.Id);
            if (row == null) return Result.Fail($"{request.Data.Name} was not found!");

           
            row.Reference = request.Data.Reference;
            row.Order = request.Data.Order;
            row.BrandId = request.Data.BrandId;
            row.UnitaryCost = request.Data.UnitaryCost;
            row.Budget = request.Data.Budget;
            row.Existing = request.Data.Existing;
            row.Model = request.Data.Model;
            row.Name = request.Data.Name;
            row.Percentage = request.Data.Percentage;
            row.Quantity = request.Data.Quantity;
            await Repository.UpdateBudgetItem(row);
            var result=await Context.SaveChangesAsync(cancellationToken);
            if(result>0)
            {
                return Result.Success($"{request.Data.Name} was updated succesfully!");
            }
            return Result.Success($"{request.Data.Name} was not updated succesfully!");
        }
    }

}
