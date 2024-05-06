namespace Application.NewFeatures.Brands.Commands
{
    public record NewBrandUpdateCommand(NewBrandUpdateRequest Data) : IRequest<IResult>;
    internal class NewBrandUpdateCommandHandler : IRequestHandler<NewBrandUpdateCommand, IResult>
    {
        private IAppDbContext _appDbContext;
        private IRepository Repository { get; set; }
        public NewBrandUpdateCommandHandler(IAppDbContext appDbContext, IRepository repository)
        {
            _appDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(NewBrandUpdateCommand request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetByIdAsync<Brand>(request.Data.BrandId);
            if(row == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Updated, ClassNames.Brand));
            }

            request.Data.FromRequest(row);

            await Repository.UpdateAsync(row);

            var result = await _appDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetAllBrands);
            
            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Updated, ClassNames.Brand)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Updated, ClassNames.Brand));


        }
    }
}
