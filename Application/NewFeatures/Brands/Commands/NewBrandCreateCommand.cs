namespace Application.NewFeatures.Brands.Commands
{
    public record NewBrandCreateCommand(NewBrandCreateRequest Data) : IRequest<IResult>;
    internal class NewBrandCreateCommandHandler : IRequestHandler<NewBrandCreateCommand, IResult>
    {
        private IAppDbContext _appDbContext;
        private IRepository Repository { get; set; }
        public NewBrandCreateCommandHandler(IAppDbContext appDbContext, IRepository repository)
        {
            _appDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(NewBrandCreateCommand request, CancellationToken cancellationToken)
        {
            var row = Brand.Create(request.Data.Name);
            await Repository.AddAsync(row);
            var result = await _appDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetAllBrands);

            return result > 0 ?
                Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Created, ClassNames.Brand)) :
                Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Created, ClassNames.Brand));
        }
    }

}
