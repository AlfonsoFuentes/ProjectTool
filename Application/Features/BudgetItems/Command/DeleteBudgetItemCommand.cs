using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Command
{
    public record DeleteBudgetItemCommand(BudgetItemResponse Data):IRequest<IResult>;
    public class DeleteBudgetItemCommandHandler:IRequestHandler<DeleteBudgetItemCommand,IResult>
    {
        private IAppDbContext _appDbContext;
        public DeleteBudgetItemCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IResult> Handle(DeleteBudgetItemCommand request, CancellationToken cancellationToken)
        {
            var row=await _appDbContext.BudgetItems.FindAsync(request.Data.Id);
            if (row==null)
            {
                return Result.Fail($"{request.Data.Name} was not found!");
                
            }

            _appDbContext.BudgetItems.Remove(row); 
            var result=await _appDbContext.SaveChangesAsync(cancellationToken); 
            if(result>0)
            {
                return Result.Success($"{request.Data.Name} was removed succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not removed succesfully!");
        }
    }

}
