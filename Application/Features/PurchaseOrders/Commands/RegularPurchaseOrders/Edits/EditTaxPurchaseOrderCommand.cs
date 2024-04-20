using Application.Features.PurchaseOrders.Validators.RegularPurchaseOrders;
using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.Taxes;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Commands.RegularPurchaseOrders.Edits
{
    public record EditTaxPurchaseOrderCommand(EditTaxPurchaseOrderRequest Data) : IRequest<IResult>;
    public class EditTaxPurchaseOrderCommandHandler : IRequestHandler<EditTaxPurchaseOrderCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        private IMWORepository MWORepository { get; set; }
        public EditTaxPurchaseOrderCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository, IMWORepository mWORepository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(EditTaxPurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            
            var purchaseorder = await Repository.GetPurchaseOrderById(request.Data.PurchaseorderId);
            if (purchaseorder == null)
            {
                return Result.Fail($"Purchase order Not found");

            }

            purchaseorder.PurchaseorderName = request.Data.PurchaseorderName;
            purchaseorder.PurchaseRequisition = $"Tax for {request.Data.PurchaseOrderItem.Name}";
            purchaseorder.QuoteCurrency = 0;
            purchaseorder.IsAlteration = false;
            purchaseorder.USDEUR = request.Data.USDEUR;
            purchaseorder.SPL = $"Tax for {request.Data.PurchaseOrderItem.Name}";

            purchaseorder.CurrencyDate = DateTime.UtcNow;
            purchaseorder.POValueCurrency = request.Data.PurchaseOrderItem.TotalValuePurchaseOrderCurrency;
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Closed.Id;
            purchaseorder.PONumber = request.Data.PONumber;
            purchaseorder.ActualCurrency = request.Data.SumPOValueCurrency;
            purchaseorder.QuoteNo = $"Tax for {request.Data.PurchaseOrderItem.Name}";
            purchaseorder.TaxCode = $"Tax for {request.Data.PurchaseOrderItem.Name}";
            purchaseorder.USDCOP = request.Data.USDCOP;
            purchaseorder.AccountAssigment = request.Data.MWO.CECName;
            purchaseorder.MainBudgetItemId = request.Data.MainBudgetItem.BudgetItemId;
            purchaseorder.Currency = request.Data.PurchaseOrderCurrency.Id;
            await Repository.UpdatePurchaseOrder(purchaseorder);
            var purchaseorderitem = await Repository.GetPurchaseOrderItemById(request.Data.PurchaseOrderItem.PurchaseOrderItemId);
            purchaseorderitem.UnitaryValueCurrency = request.Data.PurchaseOrderItem.TotalValuePurchaseOrderCurrency;
            purchaseorderitem.Quantity = 1;
            purchaseorderitem.ActualCurrency = request.Data.PurchaseOrderItem.TotalValuePurchaseOrderCurrency;
            await Repository.UpdatePurchaseOrderItem(purchaseorderitem);

            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
          
            if (result > 0)
                return Result.Success($"Purchase order created succesfully");
            return Result.Fail($"Purchase order was not created succesfully");
        }
    }
}
