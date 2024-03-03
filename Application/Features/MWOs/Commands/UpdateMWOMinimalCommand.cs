using Application.Features.MWOs.Validators;
using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;

namespace Application.Features.MWOs.Commands
{
    public record UpdateMWOMinimalCommand(UpdateMWOMinimalRequest Data) : IRequest<IResult>;
    public class UpdateMWOMinimalCommandHandler : IRequestHandler<UpdateMWOMinimalCommand, IResult>
    {
        private IMWORepository Repository { get; set; }

        private IAppDbContext AppDbContext;
        private IBudgetItemRepository RepositoryBudgetItem { get; set; }
        public UpdateMWOMinimalCommandHandler(IMWORepository repository, IAppDbContext appDbContext, IBudgetItemRepository repositoryBudgetItem)
        {
            Repository = repository;

            AppDbContext = appDbContext;
            RepositoryBudgetItem = repositoryBudgetItem;
        }

        public async Task<IResult> Handle(UpdateMWOMinimalCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateMWOMinimalValidator(Repository);
            var validatorResult = await validator.ValidateAsync(request.Data);
            if(!validatorResult.IsValid)
            {
                return Result.Fail(validatorResult.Errors.Select(x=>x.ErrorMessage).ToList());
            }

            var mwo = await AppDbContext.MWOs.FindAsync(request.Data.Id);
            if (mwo == null)
            {
                return Result.Fail($"{request.Data.Name} was not found.");
            }
            mwo.Name = request.Data.Name;
            mwo.Type = request.Data.Type.Id;

                     

            await Repository.UpdateMWO(mwo!);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            await RepositoryBudgetItem.UpdateTaxesAndEngineeringContingencyItems(mwo.Id, cancellationToken);

            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} updated succesfully");
            }

            return Result.Fail($"{request.Data.Name} was not updated succesfully");
        }
       

    }
}
