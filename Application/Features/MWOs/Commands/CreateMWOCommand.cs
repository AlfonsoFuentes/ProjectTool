using Shared.Enums.BudgetItemTypes;
using Shared.Models.MWO;

namespace Application.Features.MWOs.Commands
{
    public record CreateMWOCommand(CreateMWORequest Data) : IRequest<IResult>;

    public class CreateMWOCommandHandler : IRequestHandler<CreateMWOCommand, IResult>
    {
        private IMWORepository Repository { get; set; }
        private IBudgetItemRepository RepositoryBudgetItem { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        public CreateMWOCommandHandler(IMWORepository repository, IAppDbContext appDbContext, IBudgetItemRepository repositoryBudgetItem)
        {
            Repository = repository;

            AppDbContext = appDbContext;
            RepositoryBudgetItem = repositoryBudgetItem;
        }

        public async Task<IResult> Handle(CreateMWOCommand request, CancellationToken cancellationToken)
        {
          
            var row = MWO.Create(request.Data.Name, request.Data.Type.Id);
            row.IsAssetProductive = request.Data.IsAssetProductive;
           
            row.PercentageTaxForAlterations= request.Data.PercentageTaxForAlterations;
            
            await Repository.AddMWO(row);
            if (!row.IsAssetProductive)
            {

                await CreateTaxesForNoProductive(row, request.Data);
            }
            await CreateEngineeringItem(row, request.Data);
            await CreateContingencyItem(row, request.Data);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} created succesfully");
            }

            return Result.Fail($"{request.Data.Name} was not created succesfully");
        }
        async Task CreateTaxesForNoProductive(MWO mwo, CreateMWORequest Data)
        {
            var taxitem = mwo.AddBudgetItem(BudgetItemTypeEnum.Taxes.Id);

            taxitem.IsNotAbleToEditDelete = true;
            taxitem.Name = $"Tax non productive";
            taxitem.Percentage = Data.PercentageAssetNoProductive;
            taxitem.IsMainItemTaxesNoProductive = true;
            await RepositoryBudgetItem.AddBudgetItem(taxitem);
        }
        async Task CreateEngineeringItem(MWO mwo, CreateMWORequest Data)
        {
            var budgetItem = mwo.AddBudgetItem(BudgetItemTypeEnum.Engineering.Id);


            budgetItem.Name = $"Capitalized Salaries";
            budgetItem.Percentage = Data.PercentageEngineering;
            budgetItem.IsNotAbleToEditDelete = true;
            await RepositoryBudgetItem.AddBudgetItem(budgetItem);
        }
        async Task CreateContingencyItem(MWO mwo, CreateMWORequest Data)
        {
            var budgetItem = mwo.AddBudgetItem(BudgetItemTypeEnum.Contingency.Id);


            budgetItem.Name = $"Contingency";
            budgetItem.Percentage = Data.PercentageContingency;
            budgetItem.IsNotAbleToEditDelete = true;
            await RepositoryBudgetItem.AddBudgetItem(budgetItem);
        }
    }

}
