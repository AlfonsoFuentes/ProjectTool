using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Command
{
    public record UpdateEquipmentInstrumentsItemCommand(UpdateBudgetItemRequest Data) : IRequest<IResult>;
    public class UpdateEquipmentInstrumentsItemCommandHandler : IRequestHandler<UpdateEquipmentInstrumentsItemCommand, IResult>
    {
        private IBudgetItemRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        private IMWORepository MWORepository { get; set; }
        public UpdateEquipmentInstrumentsItemCommandHandler(IAppDbContext appDbContext, IBudgetItemRepository repository, IMWORepository mWORepository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(UpdateEquipmentInstrumentsItemCommand request, CancellationToken cancellationToken)
        {

            var row = await Repository.GetBudgetItemWithBrandById(request.Data.Id);

            row.Name = request.Data.Name;
            row.UnitaryCost = request.Data.UnitaryCost;
            row.Budget = request.Data.UnitaryCost * request.Data.Quantity;
            row.Existing = request.Data.Existing;
            row.Quantity = request.Data.Quantity;
            row.Model = request.Data.Model;
            row.Reference = request.Data.Reference;
            row.BrandId = request.Data.Brand == null ? null : request.Data.Brand.Id;

            await Repository.UpdateBudgetItem(row);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);

            await Repository.UpdateTaxesAndEngineeringContingencyItems(row.MWOId, cancellationToken);
            //await MWORepository.UpdateDataForNotApprovedMWO(row.MWOId, cancellationToken);
            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} updated succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not updated succesfully!");
        }

    }
}
