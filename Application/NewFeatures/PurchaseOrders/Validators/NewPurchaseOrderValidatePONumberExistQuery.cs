using Application.Interfaces;
using MediatR;

namespace Application.Features.PurchaseorderValidators.Queries
{
    public record NewPurchaseOrderValidatePONumberExistQuery(Guid PurchaseOrderId, string ponumber) : IRequest<bool>;
    internal class NewPurchaseOrderValidatePONumberExistQueryHandler : IRequestHandler<NewPurchaseOrderValidatePONumberExistQuery, bool>
    {
     
        private IQueryValidationsRepository QueryRepository;
        public NewPurchaseOrderValidatePONumberExistQueryHandler(IQueryValidationsRepository repository)
        {
            QueryRepository = repository;
        }

        public async Task<bool> Handle(NewPurchaseOrderValidatePONumberExistQuery request, CancellationToken cancellationToken)
        {
            return await QueryRepository.ValidatePurchaseNumberExist(request.PurchaseOrderId, request.ponumber);
        }
    }
}
