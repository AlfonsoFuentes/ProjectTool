

namespace Application.NewFeatures.Brands.Queries
{
    public record NewBrandGetAllQuery : IRequest<IResult<NewBrandListResponse>>;

    internal class NewBrandGetAllQueryHandler : IRequestHandler<NewBrandGetAllQuery, IResult<NewBrandListResponse>>
    {
        private IQueryRepository Repository { get; set; }
        private readonly IAppDbContext _cache;
        public NewBrandGetAllQueryHandler(IQueryRepository repository, IAppDbContext cache)
        {
            Repository = repository;
            _cache = cache;
        }

        public async Task<IResult<NewBrandListResponse>> Handle(NewBrandGetAllQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Brand>>> getAllBrands = () => Repository.GetAllBrandsAsync();
            try
            {
                var brandList = await _cache.GetOrAddAsync(Cache.GetAllBrands, getAllBrands);
                NewBrandListResponse responseList = new NewBrandListResponse()
                {
                    Brands = brandList.Select(x => x.ToResponse()).ToList(),
                };
                return Result<NewBrandListResponse>.Success(responseList);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
           

            return Result<NewBrandListResponse>.Fail();
        }
    }
}
