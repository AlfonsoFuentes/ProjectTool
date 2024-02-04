﻿using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Command
{
    public record CreateTaxItemCommand(CreateBudgetItemRequest Data) : IRequest<IResult>;
    public class CreateTaxItemCommandHandler : IRequestHandler<CreateTaxItemCommand, IResult>
    {
        private IBudgetItemRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public CreateTaxItemCommandHandler(IAppDbContext appDbContext, IBudgetItemRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(CreateTaxItemCommand request, CancellationToken cancellationToken)
        {
            var mwo = await Repository.GetMWOWithItemsById(request.Data.MWOId);

            if (mwo == null) return Result.Fail("MWO not found!");

            var row = mwo.AddBudgetItem(request.Data.Type.Id);
            row.Name = request.Data.Name;
            row.Percentage = request.Data.Percentage;
            //Aqui poner el calculo de Taxes

            await Repository.AddBudgetItem(row);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            await UpdateEngineeringCost(request.Data.MWOId, cancellationToken);
            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} created succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not created succesfully!");
        }
        async Task UpdateEngineeringCost(Guid MWOId, CancellationToken cancellationToken)
        {
            await Repository.UpdateEngCostContingency(MWOId);
            var resultEng = await AppDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}