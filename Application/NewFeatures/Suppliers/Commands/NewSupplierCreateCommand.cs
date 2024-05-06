

namespace Application.NewFeatures.Suppliers.Commands
{
    public record NewSupplierCreateCommand(NewSupplierCreateRequest Data) : IRequest<IResult>;
    internal class NewSupplierCreateCommandHandler : IRequestHandler<NewSupplierCreateCommand, IResult>
    {
        private IRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public NewSupplierCreateCommandHandler(IAppDbContext appDbContext, IRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(NewSupplierCreateCommand request, CancellationToken cancellationToken)
        {
            var row = Supplier.Create();
            request.Data.FromSupplierCreate(row);
            await Repository.AddAsync(row);
            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetAllSuppliers);
            return result > 0 ?
                Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Created, ClassNames.Supplier)) :
                Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Created, ClassNames.Supplier));
        }
    }
}
