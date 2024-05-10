using Application.Interfaces;

namespace Application.Features.MWOs.Commands
{
    public record NewMWODeleteCommand(NewMWODeleteRequest Data) : IRequest<IResult>;

    internal class NewMWODeleteCommandHandler : IRequestHandler<NewMWODeleteCommand, IResult>
    {
        private IAppDbContext _appDbContext;
        private IRepository Repository { get; set; }
        public NewMWODeleteCommandHandler(IAppDbContext appDbContext, IRepository repository)
        {
            _appDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(NewMWODeleteCommand request, CancellationToken cancellationToken)
        {
            var mwo = await Repository.GetByIdAsync<MWO>(request.Data.MWOId);
            if (mwo == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.NotFound, ClassNames.MWO));
            }


            await Repository.RemoveAsync(mwo);
 
            var result = await _appDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCacheMWO(mwo));

            return result > 0 ?
               Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Delete, ClassNames.MWO)) :
               Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Delete, ClassNames.MWO));


        }
    }

}
