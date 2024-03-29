using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Command
{
    public record CreateAlterationBudgetItemCommand(CreateBudgetItemRequestDto Data) : IRequest<IResult>;
    public class CreateAlterationBudgetItemCommandHandler : IRequestHandler<CreateAlterationBudgetItemCommand, IResult>
    {
        private IBudgetItemRepository Repository { get; set; }
        private IMWORepository MWORepository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public CreateAlterationBudgetItemCommandHandler(IAppDbContext appDbContext, IBudgetItemRepository repository, IMWORepository mWORepository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(CreateAlterationBudgetItemCommand request, CancellationToken cancellationToken)
        {
           
            var mwo = await Repository.GetMWOWithItemsById(request.Data.MWOId);

            if (mwo == null) return Result.Fail("MWO not found!");

            var row = mwo.AddBudgetItem(request.Data.Type);
            row.Name = request.Data.Name;
            row.UnitaryCost = request.Data.UnitaryCost;
            row.Budget = request.Data.UnitaryCost * request.Data.Quantity;

            row.Quantity = request.Data.Quantity;



            await Repository.AddBudgetItem(row);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            await MWORepository.UpdateDataForNotApprovedMWO(mwo.Id, cancellationToken);
            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} created succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not created succesfully!");
        }

    }
}
