using Application.Features.PurchaseOrders.Validators;
using Application.Features.PurchaseOrders.Validators.RegularPurchaseOrders;
using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Commands.RegularPurchaseOrders.Creates
{
    public record ApproveRegularPurchaseOrderCommand(ApprovedRegularPurchaseOrderRequestDto Data) : IRequest<IResult>;


    internal class ApproveRegularPurchaseOrderCommandHandler : IRequestHandler<ApproveRegularPurchaseOrderCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public ApproveRegularPurchaseOrderCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(ApproveRegularPurchaseOrderCommand request, CancellationToken cancellationToken)
        {
           

            var purchaseorder = await Repository.GetPurchaseOrderToEditById(request.Data.PurchaseOrderId);
            if (purchaseorder == null)
            {
                return Result.Fail($"Not found data approving purchase order: {request.Data.PONumber}");

            }
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Approved.Id;
            purchaseorder.PONumber = request.Data.PONumber;
            purchaseorder.POExpectedDateDate = request.Data.ExpectedDate!.Value;

            if (request.Data.IsMWONoProductive && !request.Data.IsAlteration)
            {
                var TaxBudgetitem = await Repository.GetTaxBudgetItemNoProductive(purchaseorder.MWOId);
                if (TaxBudgetitem != null)
                {
                    var sumPOValueUSD = purchaseorder.POValueUSD;

                    var purchaseordertaxestem = purchaseorder.AddPurchaseOrderItemForNoProductiveTax(TaxBudgetitem.Id,
                            $"{request.Data.PONumber} Tax {TaxBudgetitem.Percentage}%");
                    purchaseordertaxestem.POValueUSD = TaxBudgetitem.Percentage / 100.0 * sumPOValueUSD;
                    await Repository.AddPurchaseorderItem(purchaseordertaxestem);

                }

            }


            await Repository.UpdatePurchaseOrder(purchaseorder);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Result.Success($"Purchase order: {purchaseorder.PONumber} was approved succesfully"); ;
            }

            return Result.Fail($"Purchase order: {purchaseorder.PONumber} was not approved succesfully");
        }
    }
}
