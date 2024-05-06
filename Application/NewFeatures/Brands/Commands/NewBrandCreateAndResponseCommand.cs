

namespace Application.NewFeatures.Brands.Commands
{
    public record NewBrandCreateAndResponseCommand(NewBrandCreateRequest Data) : IRequest<IResult<NewBrandResponse>>;
    internal class NewBrandCreateAndResponseCommandHandler : IRequestHandler<NewBrandCreateAndResponseCommand, IResult<NewBrandResponse>>
    {
        private IAppDbContext _appDbContext;
        private IRepository Repository { get; set; }
        public NewBrandCreateAndResponseCommandHandler(IAppDbContext appDbContext, IRepository repository)
        {
            _appDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult<NewBrandResponse>> Handle(NewBrandCreateAndResponseCommand request, CancellationToken cancellationToken)
        {
            var row = Brand.Create(request.Data.Name);
            await Repository.AddAsync(row);
            var result = await _appDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetAllBrands);

            NewBrandResponse response = row.ToResponse();
            return result > 0 ?
               Result<NewBrandResponse>.Success(response, ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Created, ClassNames.Brand)) :
               Result<NewBrandResponse>.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Created, ClassNames.Brand));

        }
    }
}
