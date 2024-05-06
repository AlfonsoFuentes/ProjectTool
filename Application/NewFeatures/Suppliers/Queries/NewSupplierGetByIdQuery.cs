namespace Application.NewFeatures.Suppliers.Queries
{
    public record NewSupplierGetByIdToUpdateQuery(Guid SupplierId) : IRequest<IResult<NewSupplierUpdateRequest>>;
    public class NewSupplierGetByIdToUpdateQueryHandler : IRequestHandler<NewSupplierGetByIdToUpdateQuery, IResult<NewSupplierUpdateRequest>>
    {
        private IQueryRepository Repository { get; set; }

        public NewSupplierGetByIdToUpdateQueryHandler(IQueryRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<NewSupplierUpdateRequest>> Handle(NewSupplierGetByIdToUpdateQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetSupplierByIdAsync(request.SupplierId);

            if (row == null) { return Result<NewSupplierUpdateRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.Supplier)); }

            NewSupplierUpdateRequest response = row.ToUpdateRequest();

            return Result<NewSupplierUpdateRequest>.Success(response);
        }
    }
}
