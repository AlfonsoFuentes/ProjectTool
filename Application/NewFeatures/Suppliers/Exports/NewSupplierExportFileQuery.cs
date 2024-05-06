namespace Application.NewFeatures.Suppliers.Exports
{
    public record NewSupplierExportFileQuery() : IRequest<IResult<NewSupplierExportFileListResponse>>;
    public class NewSupplierExportFileQueryHandler : IRequestHandler<NewSupplierExportFileQuery, IResult<NewSupplierExportFileListResponse>>
    {
        private IQueryRepository Repository { get; set; }
        private readonly IAppDbContext _cache;
        public NewSupplierExportFileQueryHandler(IQueryRepository repository, IAppDbContext cache)
        {
            Repository = repository;
            _cache = cache;
        }

        public async Task<IResult<NewSupplierExportFileListResponse>> Handle(NewSupplierExportFileQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Supplier>>> getAll = () => Repository.GetAllSuppliersAsync();

            try
            {
                var list = await _cache.GetOrAddAsync(Cache.GetAllSuppliers, getAll);

                NewSupplierExportFileListResponse responseList = new NewSupplierExportFileListResponse()
                {
                    Suppliers = list.Select(x => x.ToFileExportResponse()).AsQueryable(),
                };

                return Result<NewSupplierExportFileListResponse>.Success(responseList);

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return Result<NewSupplierExportFileListResponse>.Fail(ResponseMessages.ReponseFailMessage("Excel suppliers", ResponseType.Created, ClassNames.Supplier));


        }
    }

}
