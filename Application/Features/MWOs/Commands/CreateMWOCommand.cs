﻿using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;

namespace Application.Features.MWOs.Commands
{
    public record CreateMWOCommand(CreateMWORequestDto Data) : IRequest<IResult>;

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
          
            var row = MWO.Create(request.Data.Name, request.Data.Type);
            row.IsAssetProductive = request.Data.IsAssetProductive;
            row.PercentageEngineering = request.Data.PercentageEngineering;
            row.PercentageContingency = request.Data.PercentageContingency;
            row.PercentageTaxForAlterations= request.Data.PercentageTaxForAlterations;
            if (!row.IsAssetProductive)
            {
                row.PercentageAssetNoProductive = request.Data.PercentageAssetNoProductive;
                await CreateTaxesForNoProductive(row, request.Data);
            }
            await Repository.AddMWO(row);
            await CreateEngineeringItem(row, request.Data);
            await CreateContingencyItem(row, request.Data);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} created succesfully");
            }

            return Result.Fail($"{request.Data.Name} was not created succesfully");
        }
        async Task CreateTaxesForNoProductive(MWO mwo, CreateMWORequestDto Data)
        {
            var taxitem = mwo.AddBudgetItem(BudgetItemTypeEnum.Taxes.Id);

            taxitem.IsNotAbleToEditDelete = true;
            taxitem.Name = $"Tax non productive";
            taxitem.Percentage = Data.PercentageAssetNoProductive;
            taxitem.IsMainItemTaxesNoProductive = true;
            await RepositoryBudgetItem.AddBudgetItem(taxitem);
        }
        async Task CreateEngineeringItem(MWO mwo, CreateMWORequestDto Data)
        {
            var budgetItem = mwo.AddBudgetItem(BudgetItemTypeEnum.Engineering.Id);


            budgetItem.Name = $"Capitalized Salaries";
            budgetItem.Percentage = Data.PercentageEngineering;
            budgetItem.IsNotAbleToEditDelete = true;
            await RepositoryBudgetItem.AddBudgetItem(budgetItem);
        }
        async Task CreateContingencyItem(MWO mwo, CreateMWORequestDto Data)
        {
            var budgetItem = mwo.AddBudgetItem(BudgetItemTypeEnum.Contingency.Id);


            budgetItem.Name = $"Contingency";
            budgetItem.Percentage = Data.PercentageContingency;
            budgetItem.IsNotAbleToEditDelete = true;
            await RepositoryBudgetItem.AddBudgetItem(budgetItem);
        }
    }

}
