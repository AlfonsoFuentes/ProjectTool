using Shared.Enums.Currencies;
using Shared.Enums.PurchaseorderStatus;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;

namespace Application.Features.PurchaseOrders.Commands.RegularPurchaseOrders.Edits
{
    public record EditPurchaseOrderCapitalizedSalaryCommand(EditCapitalizedSalaryPurchaseOrderRequest Data) : IRequest<IResult>;
    internal class EditPurchaseOrderCapitalizedSalaryCommandHandler : IRequestHandler<EditPurchaseOrderCapitalizedSalaryCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        private IMWORepository MWORepository { get; set; }
        public EditPurchaseOrderCapitalizedSalaryCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository, IMWORepository mWORepository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(EditPurchaseOrderCapitalizedSalaryCommand request, CancellationToken cancellationToken)
        {

            var purchaseorder = await Repository.GetPurchaseOrderById(request.Data.PurchaseOrderId);
            if (purchaseorder == null)
            {
                return Result.Fail($"Purchase order Not found");

            }

            purchaseorder.PurchaseorderName = request.Data.PurchaseOrderName;
            purchaseorder.PurchaseRequisition = request.Data.PurchaseorderNumber;
            purchaseorder.PONumber = request.Data.PurchaseorderNumber;
            purchaseorder.QuoteCurrency = CurrencyEnum.USD.Id;
            purchaseorder.IsAlteration = false;
            purchaseorder.USDEUR = request.Data.USDEUR;
            purchaseorder.USDCOP = request.Data.USDCOP;
            purchaseorder.SPL = "";

            purchaseorder.CurrencyDate = DateTime.UtcNow;
            //purchaseorder.POValueCurrency = request.Data.SumPOValueCurrency;
            //purchaseorder.ActualCurrency = request.Data.SumPOValueCurrency;
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Closed.Id;
            purchaseorder.QuoteNo = "";
            purchaseorder.TaxCode = "";
            purchaseorder.POClosedDate = DateTime.UtcNow;
            purchaseorder.AccountAssigment = request.Data.MWOCECName;
            purchaseorder.MainBudgetItemId = request.Data.MainBudgetItemId;
            purchaseorder.PurchaseOrderCurrency = CurrencyEnum.USD.Id;
            await Repository.UpdatePurchaseOrder(purchaseorder);
            var purchaseorderitem = await Repository.GetPurchaseOrderItemById(request.Data.PurchaseOrderItem.PurchaseOrderItemId);
            purchaseorderitem.UnitaryValueCurrency = request.Data.SumPOValueCurrency;
            purchaseorderitem.Quantity = 1;
            purchaseorderitem.ActualCurrency = request.Data.SumPOValueCurrency;

            await Repository.UpdatePurchaseOrderItem(purchaseorderitem);

            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
          
            if (result > 0)
                return Result.Success($"Purchase order created succesfully");
            return Result.Fail($"Purchase order was not created succesfully");
        }
    }
}
