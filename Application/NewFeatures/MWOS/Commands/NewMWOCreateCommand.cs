namespace Application.NewFeatures.MWOS.Commands
{

    public record NewMWOCreateCommand(NewMWOCreateRequest Data):IRequest<IResult>;

    internal class NewMWOCreateCommandHandler:IRequestHandler<NewMWOCreateCommand,IResult>
    {
        IAppDbContext appDbContext;
        IRepository repository;

        public NewMWOCreateCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            this.repository = repository;
            this.appDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewMWOCreateCommand request, CancellationToken cancellationToken)
        {
            var row = MWO.Create();

            request.Data.FromMWOCreateRequest(row);
            if (!row.IsAssetProductive)
            {
                
                await CreateTaxesForNoProductive(row, request.Data);
            }
           
            await CreateEngineeringItem(row, request.Data);
            await CreateContingencyItem(row, request.Data);
            await repository.AddAsync(row);
       
            var result = await appDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCacheMWO(row.Id));
            return result > 0 ?
               Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Created, ClassNames.MWO)) :
               Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Created, ClassNames.MWO));

        }
        async Task CreateTaxesForNoProductive(MWO mwo, NewMWOCreateRequest Data)
        {
            var taxitem = mwo.AddBudgetItem(BudgetItemTypeEnum.Taxes.Id);

            taxitem.IsNotAbleToEditDelete = true;
            taxitem.Name = $"Tax non productive";
            taxitem.Percentage = Data.PercentageAssetNoProductive;
            taxitem.Quantity = 1;
            taxitem.IsMainItemTaxesNoProductive = true;
            await repository.AddAsync(taxitem);
        }
        async Task CreateEngineeringItem(MWO mwo, NewMWOCreateRequest Data)
        {
            var budgetItem = mwo.AddBudgetItem(BudgetItemTypeEnum.Engineering.Id);

            budgetItem.IsEngineeringItem = true;
            budgetItem.Name = $"Capitalized Salaries";
            budgetItem.Percentage = Data.PercentageEngineering;
            budgetItem.IsNotAbleToEditDelete = true;
            budgetItem.Quantity = 1;
            await repository.AddAsync(budgetItem);
        }
        async Task CreateContingencyItem(MWO mwo, NewMWOCreateRequest Data)
        {
            var budgetItem = mwo.AddBudgetItem(BudgetItemTypeEnum.Contingency.Id);

            budgetItem.IsEngineeringItem = true;
            budgetItem.Name = $"Contingency";
            budgetItem.Percentage = Data.PercentageContingency;
            budgetItem.IsNotAbleToEditDelete = true;
            budgetItem.Quantity = 1;
            await repository.AddAsync(budgetItem);
        }
    }
}
