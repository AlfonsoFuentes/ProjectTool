using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Command
{
    public record CreateEngContingencyCommand(CreateBudgetItemRequest Data) : IRequest<IResult>;
    public class CreateEngContingencyCommandHandler : IRequestHandler<CreateEngContingencyCommand, IResult>
    {
        private IBudgetItemRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        private IMWORepository MWORepository { get; set; }
        public CreateEngContingencyCommandHandler(IAppDbContext appDbContext, IBudgetItemRepository repository, IMWORepository mWORepository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(CreateEngContingencyCommand request, CancellationToken cancellationToken)
        {
           
            var mwo = await Repository.GetMWOWithItemsById(request.Data.MWOId);

            if (mwo == null) return Result.Fail("MWO not found!");

            var row = mwo.AddBudgetItem(request.Data.Type.Id);
            row.Name = request.Data.Name;
            row.Percentage = request.Data.Percentage;
            row.UnitaryCost = request.Data.UnitaryCost;
            row.Quantity = request.Data.Quantity;
       

            if (!mwo.IsAssetProductive && request.Data.Budget > 0 && request.Data.Percentage == 0)
            {
                var MWOtaxItem = await Repository.GetMainBudgetTaxItemByMWO(request.Data.MWOId);

                var taxItem = MWOtaxItem.AddTaxItem(row.Id);
                await Repository.AddTaxSelectedItem(taxItem);
            }

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
