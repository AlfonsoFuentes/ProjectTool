

namespace Application.NewFeatures.Suppliers.Commands
{
    public record NewSupplierCreateAndResponseCommand(NewSupplierCreateBasicRequest Data) : IRequest<IResult<NewSupplierResponse>>;
    internal class NewSupplierCreateBasicAndResponseCommandHandler : IRequestHandler<NewSupplierCreateAndResponseCommand, IResult<NewSupplierResponse>>
    {
        private IRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public NewSupplierCreateBasicAndResponseCommandHandler(IAppDbContext appDbContext, IRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult<NewSupplierResponse>> Handle(NewSupplierCreateAndResponseCommand request, CancellationToken cancellationToken)
        {
            var row = Supplier.Create();
            request.Data.FromSupplierCreateBasic(row);
            await Repository.AddAsync(row);
            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetAllSuppliers);
            var response=row.ToResponse();
            return result > 0 ?
                Result<NewSupplierResponse>.Success(response,ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Created, ClassNames.Supplier)) :
                Result<NewSupplierResponse>.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Created, ClassNames.Supplier));
        }
    }
}
