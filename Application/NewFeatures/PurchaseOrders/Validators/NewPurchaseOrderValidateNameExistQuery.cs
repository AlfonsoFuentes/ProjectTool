namespace Application.Features.PurchaseorderValidators.Queries
{
    public record NewPurchaseOrderValidateNameExistQuery(Guid MWOId,Guid PurchaseOrderId, string name) : IRequest<bool>;

    internal class NewPurchaseOrderValidateNameExistQueryHandler : IRequestHandler<NewPurchaseOrderValidateNameExistQuery, bool>
    {
     
        private IQueryValidationsRepository QueryRepository;
        public NewPurchaseOrderValidateNameExistQueryHandler(IQueryValidationsRepository queryRepository)
        {
           
            QueryRepository = queryRepository;
        }

        public async Task<bool> Handle(NewPurchaseOrderValidateNameExistQuery request, CancellationToken cancellationToken)
        {
            return await QueryRepository.ValidatePurchaseOrderNameExist(request.MWOId, request.PurchaseOrderId, request.name);
        }
    }

}
