namespace Application.Features.PurchaseorderValidators.Queries
{
    public record NewPurchaseOrderValidatePONumberQuery(string ponumber) : IRequest<bool>;
    internal class NewPurchaseOrderValidatePONumberQueryHandler : IRequestHandler<NewPurchaseOrderValidatePONumberQuery, bool>
    {
        
        private IQueryValidationsRepository QueryRepository;
        public NewPurchaseOrderValidatePONumberQueryHandler(IQueryValidationsRepository repository)
        {
            QueryRepository = repository;
        }

        public async Task<bool> Handle(NewPurchaseOrderValidatePONumberQuery request, CancellationToken cancellationToken)
        {
            return await QueryRepository.ValidatePurchaseNumberExist(request.ponumber);
        }
    }
}
