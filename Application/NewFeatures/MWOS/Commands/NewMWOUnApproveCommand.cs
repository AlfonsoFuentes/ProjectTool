using Shared.Enums.MWOStatus;

namespace Application.NewFeatures.MWOS.Commands
{
    public record NewMWOUnApproveCommand(NewMWOUnApproveRequest Data) : IRequest<IResult>;
    internal class NewMWOUnApproveCommandHandler : IRequestHandler<NewMWOUnApproveCommand, IResult>
    {
        IAppDbContext appDbContext;
        IRepository repository;

        public NewMWOUnApproveCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            this.repository = repository;
            this.appDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewMWOUnApproveCommand request, CancellationToken cancellationToken)
        {
            var row = await repository.GetMWOById(request.Data.MWOId);
            if (row == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.NotFound, ClassNames.MWO));
            }
            row.Status = MWOStatusEnum.Created.Id;
         
            await repository.UpdateAsync(row);
            var result = await appDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCacheMWO(row));
            return result > 0 ?
               Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.UnApprove, ClassNames.MWO)) :
               Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.UnApprove, ClassNames.MWO));

        }
       
    }
}
