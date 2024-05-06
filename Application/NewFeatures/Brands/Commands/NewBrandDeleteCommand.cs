namespace Application.NewFeatures.Brands.Commands
{
    public record NewBrandDeleteCommand(NewBrandResponse Data) : IRequest<IResult>;
    internal class NewBrandDeleteCommandHandler : IRequestHandler<NewBrandDeleteCommand, IResult>
    {
        private IAppDbContext _appDbContext;
        private IRepository Repository { get; set; }
        public NewBrandDeleteCommandHandler(IAppDbContext appDbContext, IRepository repository)
        {
            _appDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(NewBrandDeleteCommand request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetByIdAsync<Brand>(request.Data.BrandId);
            if (row == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Delete, ClassNames.Brand));
            }
            await Repository.RemoveAsync(row);
            var result = await _appDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetAllBrands);

            return result > 0 ?
               Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Delete, ClassNames.Brand)) :
               Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Delete, ClassNames.Brand));


        }
    }
}
