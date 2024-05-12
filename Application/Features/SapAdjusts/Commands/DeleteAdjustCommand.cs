namespace Application.Features.SapAdjusts.Commands
{
    public record DeleteSapAdjustCommand(SapAdjustResponse Data) : IRequest<IResult>;
    internal class DeleteSapAdjustCommandHandler : IRequestHandler<DeleteSapAdjustCommand, IResult>
    {
        private IRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        public DeleteSapAdjustCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(DeleteSapAdjustCommand request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetByIdAsync<SapAdjust>(request.Data.SapAdjustId);
     
            if (row == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.NotFound, ClassNames.SapAdjust));
            }


            await Repository.RemoveAsync(row);
            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, $"{Cache.GetSapAdjust}:{request.Data.MWOId}");

            return result > 0 ?
               Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Delete, ClassNames.SapAdjust)) :
               Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Delete, ClassNames.SapAdjust));
        }
    }
}
