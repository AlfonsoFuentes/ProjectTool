using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Command
{
    public record UpdateAlterationBudgetItemCommand(UpdateBudgetItemRequestDto Data) : IRequest<IResult>;
    public class UpdateAlterationBudgetItemCommandHandler : IRequestHandler<UpdateAlterationBudgetItemCommand, IResult>
    {
        private IBudgetItemRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        private IMWORepository MWORepository { get; set; }
        public UpdateAlterationBudgetItemCommandHandler(IAppDbContext appDbContext, IBudgetItemRepository repository, IMWORepository mWORepository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(UpdateAlterationBudgetItemCommand request, CancellationToken cancellationToken)
        {
           
            var row = await Repository.GetBudgetItemById(request.Data.Id);
            row.Name = request.Data.Name;
            row.UnitaryCost = request.Data.UnitaryCost;
            row.Budget = request.Data.UnitaryCost * request.Data.Quantity;
            row.Quantity = request.Data.Quantity;



            await Repository.UpdateBudgetItem(row);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            await MWORepository.UpdateDataForNotApprovedMWO(row.MWOId, cancellationToken);

            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} updated succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not updated succesfully!");
        }

    }
}
