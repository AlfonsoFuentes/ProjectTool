namespace Application.NewFeatures.Suppliers.Commands
{
    public record NewSupplierUpdateCommand(NewSupplierUpdateRequest Data) : IRequest<IResult>;

    internal class NewSupplierUpdateCommandHandler : IRequestHandler<NewSupplierUpdateCommand, IResult>
    {
        private IRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public NewSupplierUpdateCommandHandler(IAppDbContext appDbContext, IRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(NewSupplierUpdateCommand request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetByIdAsync<Supplier>(request.Data.SupplierId);
            
            if (row == null) { return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.NotFound, ClassNames.Supplier)); }

            request.Data.FromSupplierUpdate(row);

            await Repository.UpdateAsync(row);

            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetAllSuppliers);
            return result > 0 ?
                Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Updated, ClassNames.Supplier)) :
                Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Updated, ClassNames.Supplier));
        }
    }
}
