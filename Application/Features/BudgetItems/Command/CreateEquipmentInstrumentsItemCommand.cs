﻿using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Command
{
    public record CreateEquipmentInstrumentsItemCommand(CreateBudgetItemRequest Data) : IRequest<IResult>;
    public class CreateEquipmentInstrumentsItemCommandHandler : IRequestHandler<CreateEquipmentInstrumentsItemCommand, IResult>
    {
        private IBudgetItemRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        private IMWORepository MWORepository { get; set; }
        public CreateEquipmentInstrumentsItemCommandHandler(IAppDbContext appDbContext, IBudgetItemRepository repository, IMWORepository mWORepository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(CreateEquipmentInstrumentsItemCommand request, CancellationToken cancellationToken)
        {

            var mwo = await Repository.GetMWOWithItemsById(request.Data.MWOId);

            if (mwo == null) return Result.Fail("MWO not found!");

            var row = mwo.AddBudgetItem(request.Data.Type.Id);
            row.Name = request.Data.Name;
            row.UnitaryCost = request.Data.UnitaryCost;
      
            row.Existing = request.Data.Existing;
            row.Quantity = request.Data.Quantity;
            row.Model = request.Data.Model;
            row.Reference = request.Data.Reference;
            row.BrandId = request.Data.Brand == null ? null : request.Data.Brand.BrandId;
            if (!mwo.IsAssetProductive)
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
