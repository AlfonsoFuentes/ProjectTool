using Shared.Enums.MWOStatus;
using Shared.Models.MWO;

namespace Application.Features.MWOs.Commands
{
    public record UnApproveMWOCommand(UnApproveMWORequest Data) : IRequest<IResult>;
    public class UnApproveMWOCommandHandler : IRequestHandler<UnApproveMWOCommand, IResult>
    {
        private IMWORepository Repository { get; set; }
        private IBudgetItemRepository RepositoryBudgetItem { get; set; }
        private IAppDbContext AppDbContext;
        public UnApproveMWOCommandHandler(IMWORepository repository, IAppDbContext appDbContext, IBudgetItemRepository repositoryBudgetItem)
        {
            Repository = repository;

            AppDbContext = appDbContext;
            RepositoryBudgetItem = repositoryBudgetItem;
        }

        public async Task<IResult> Handle(UnApproveMWOCommand request, CancellationToken cancellationToken)
        {

            var mwo = await Repository.GetMWOById(request.Data.MWOId);
            if (mwo == null)
            {
                return Result.Fail($"{request.Data.Name} was not found.");
            }
            mwo.Status = MWOStatusEnum.Created.Id;
            
            await Repository.UpdateMWO(mwo);

            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            
            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} un approved succesfully");
            }

            return Result.Fail($"{request.Data.Name} was not un approved succesfully");
        }
        
    }
}
