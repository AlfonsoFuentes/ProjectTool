using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;
using System;

namespace Application.Features.BudgetItems.Command
{
    public record DeleteBudgetItemCommand(BudgetItemResponse Data) : IRequest<IResult>;
    public class DeleteBudgetItemCommandHandler : IRequestHandler<DeleteBudgetItemCommand, IResult>
    {
        private IAppDbContext _appDbContext;
        private IBudgetItemRepository Repository { get; set; }
        private IMWORepository MWORepository { get; set; }
        public DeleteBudgetItemCommandHandler(IAppDbContext appDbContext, IBudgetItemRepository repository, IMWORepository mWORepository)
        {
            _appDbContext = appDbContext;
            Repository = repository;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(DeleteBudgetItemCommand request, CancellationToken cancellationToken)
        {
            var row = await _appDbContext.BudgetItems
                .Include(x => x.TaxesItems)
                .SingleOrDefaultAsync(x => x.Id == request.Data.Id);
           
            if (row == null)
            {
                return Result.Fail($"{request.Data.Name} was not found!");

            }
            if(row.IsMainItemTaxesNoProductive)
            {
                return Result.Fail($"{request.Data.Name} can not remove because is taxes non productive!");
            }
            var rowTaxes = await _appDbContext.TaxesItems
                .SingleOrDefaultAsync(x => x.SelectedId == request.Data.Id);
            if (rowTaxes!=null){

                _appDbContext.TaxesItems.Remove(rowTaxes!);
            }

            _appDbContext.BudgetItems.Remove(row);

            var result = await _appDbContext.SaveChangesAsync(cancellationToken);
            await Repository.UpdateTaxesAndEngineeringContingencyItems(row.MWOId, cancellationToken);
            //await MWORepository.UpdateDataForNotApprovedMWO(row.MWOId, cancellationToken);
            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} was removed succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not removed succesfully!");
        }

    }

}
