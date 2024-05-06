namespace Application.NewFeatures.Suppliers.Commands
{
    public record NewSupplierDeleteCommand(NewSupplierResponse Data) : IRequest<IResult>;
    internal class NewSupplierDeleteCommandHandler : IRequestHandler<NewSupplierDeleteCommand, IResult>
    {
        private IAppDbContext _appDbContext;
        private IRepository Repository { get; set; }
        public NewSupplierDeleteCommandHandler(IAppDbContext appDbContext, IRepository repository)
        {
            _appDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(NewSupplierDeleteCommand request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetByIdAsync<Supplier>(request.Data.SupplierId);

            if (row == null) { return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.NotFound, ClassNames.Supplier)); }

            await Repository.RemoveAsync(row);

            var result = await _appDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetAllSuppliers);

            return result > 0 ?
               Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Delete, ClassNames.Supplier)) :
               Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Delete, ClassNames.Supplier));


        }
    }
}
