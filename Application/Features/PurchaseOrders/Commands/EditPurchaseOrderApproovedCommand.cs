using Application.Features.PurchaseOrders.Validators;
using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.Approves;
using Shared.Models.PurchaseorderStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PurchaseOrders.Commands
{
    public record EditPurchaseOrderApproovedCommand(ApprovePurchaseOrderRequest Data) : IRequest<IResult>;


    internal class EditPurchaseOrderApproovedCommandHandler:IRequestHandler<EditPurchaseOrderApproovedCommand,IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public EditPurchaseOrderApproovedCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(EditPurchaseOrderApproovedCommand request, CancellationToken cancellationToken)
        {
            var validator = new ApprovePurchaseOrderValidator(Repository);
            var validationResult = await validator.ValidateAsync(request.Data, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList());

            }
            
            var purchaseorder = await Repository.GetPurchaseOrderById(request.Data.PurchaseorderId);
            if (purchaseorder == null)
            {
                return Result.Fail($"Not found data approving purchase order: {request.Data.PONumber}");

            }
            
            purchaseorder.PONumber = request.Data.PONumber;
            purchaseorder.POExpectedDateDate = request.Data.ExpetedOn!.Value;
            
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
