using Application.Features.PurchaseOrders.Validators;
using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.Approves;
using Shared.Models.PurchaseOrders.Requests.Closeds;

namespace Application.Features.PurchaseOrders.Commands
{
    public record EditPurchaseOrderClosedCommand(ClosedPurchaseOrderRequest Data) : IRequest<IResult>;
    internal class EditPurchaseOrderClosedCommandHandler : IRequestHandler<EditPurchaseOrderClosedCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public EditPurchaseOrderClosedCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(EditPurchaseOrderClosedCommand request, CancellationToken cancellationToken)
        {
            
            var purchaseorder = await Repository.GetPurchaseOrderClosedById(request.Data.PurchaseorderId);
            if (purchaseorder == null)
            {
                return Result.Fail($"Not found data approving purchase order: {request.Data.PONumber}");

            }

         
            purchaseorder.POExpectedDateDate = request.Data.ExpetedOn!.Value;
            purchaseorder.PurchaseorderName=request.Data.PurchaseorderName;
            await Repository.UpdatePurchaseOrder(purchaseorder);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Result.Success($"Purchase order: {purchaseorder.PONumber} was updated succesfully"); ;
            }

            return Result.Fail($"Purchase order: {purchaseorder.PONumber} was not updated succesfully");
        }
    }
}
