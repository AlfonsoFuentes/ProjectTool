namespace Application.Features.PurchaseorderValidators.Queries
{
    public record NewPurchaseOrderValidatePRNumberExistQuery(Guid PurchaseOrderId,string purchaserequisition) : IRequest<bool>;
    internal class NewPurchaseOrderValidatePRNumberExistQueryHandler : IRequestHandler<NewPurchaseOrderValidatePRNumberExistQuery, bool>
    {
      
        private IQueryValidationsRepository QueryRepository;
        public NewPurchaseOrderValidatePRNumberExistQueryHandler(IQueryValidationsRepository repository)
        {
            QueryRepository = repository;
        }

        public async Task<bool> Handle(NewPurchaseOrderValidatePRNumberExistQuery request, CancellationToken cancellationToken)
        {
            return await QueryRepository.ValidatePurchaseRequisitionExist(request.PurchaseOrderId, request.purchaserequisition);
        }
    }
}
