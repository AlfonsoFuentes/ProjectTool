namespace Application.NewFeatures.Suppliers.Queries
{
    public record NewSupplierGetAllQuery:IRequest<IResult<NewSupplierListResponse>>;

    internal class NewSupplierGetAllQueryHandler:IRequestHandler<NewSupplierGetAllQuery, IResult<NewSupplierListResponse>>
    {
        private IQueryRepository Repository { get; set; }
        private readonly IAppDbContext _cache;
        public NewSupplierGetAllQueryHandler(IQueryRepository repository, IAppDbContext cache)
        {
            Repository = repository;
            _cache = cache;
        }

        public async Task<IResult<NewSupplierListResponse>> Handle(NewSupplierGetAllQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Supplier>>> getAll = () => Repository.GetAllSuppliersAsync();
            try
            {
                var list = await _cache.GetOrAddAsync(Cache.GetAllSuppliers, getAll);
                NewSupplierListResponse responseList = new NewSupplierListResponse()
                {
                    Suppliers = list.Select(x => x.ToResponse()).ToList(),
                };
                return Result<NewSupplierListResponse>.Success(responseList);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }


            return Result<NewSupplierListResponse>.Fail();

            
        }
    }
}
