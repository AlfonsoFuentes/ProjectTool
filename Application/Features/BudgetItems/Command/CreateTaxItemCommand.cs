using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Command
{
    public record CreateTaxItemCommand(CreateBudgetItemRequestDto Data) : IRequest<IResult>;
    public class CreateTaxItemCommandHandler : IRequestHandler<CreateTaxItemCommand, IResult>
    {
        private IBudgetItemRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        private IMWORepository MWORepository { get; set; }
        public CreateTaxItemCommandHandler(IAppDbContext appDbContext, IBudgetItemRepository repository, IMWORepository mWORepository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(CreateTaxItemCommand request, CancellationToken cancellationToken)
        {
           
            var mwo = await Repository.GetMWOWithItemsById(request.Data.MWOId);

            if (mwo == null) return Result.Fail("MWO not found!");

            var row = mwo.AddBudgetItem(request.Data.Type);
            row.Name = request.Data.Name;
            row.Percentage = request.Data.Percentage;
            double sumBudget = 0;
            foreach (var itemdto in request.Data.BudgetItemDtos)
            {
                sumBudget += itemdto.Budget * row.Percentage / 100.0;
                var taxItem = row.AddTaxItem(itemdto.BudgetItemId);
                await Repository.AddTaxSelectedItem(taxItem);
            }
            row.Budget = sumBudget;
            await Repository.AddBudgetItem(row);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            await Repository.UpdateTaxesAndEngineeringContingencyItems(row.MWOId, cancellationToken);
            await MWORepository.UpdateDataForNotApprovedMWO(mwo.Id, cancellationToken);
            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} created succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not created succesfully!");
        }
        
    }
}
