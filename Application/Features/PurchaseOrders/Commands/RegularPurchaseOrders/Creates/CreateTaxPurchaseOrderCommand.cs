using Shared.Enums.PurchaseorderStatus;
using Shared.Models.PurchaseOrders.Requests.Taxes;

namespace Application.Features.PurchaseOrders.Commands.RegularPurchaseOrders.Creates
{
    public record CreateTaxPurchaseOrderCommand(CreateTaxPurchaseOrderRequest Data) : IRequest<IResult>;
    public class CreateTaxPurchaseOrderCommandHandler : IRequestHandler<CreateTaxPurchaseOrderCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        private IMWORepository MWORepository { get; set; }
        public CreateTaxPurchaseOrderCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository, IMWORepository mWORepository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(CreateTaxPurchaseOrderCommand request, CancellationToken cancellationToken)
        {
           
            var mwo = await Repository.GetMWOWithBudgetItemsAndPurchaseOrderById(request.Data.MWOId);
            if (mwo == null)
            {
                return Result.Fail($"MWO Not found");

            }
            var purchaseorder = mwo.AddPurchaseOrder();
            purchaseorder.PurchaseorderName = request.Data.Name;
           
            purchaseorder.QuoteCurrency = request.Data.PurchaseOrderCurrency.Id;
            purchaseorder.IsAlteration = false;
            purchaseorder.USDEUR = request.Data.USDEUR;
            purchaseorder.USDCOP = request.Data.USDCOP;
            purchaseorder.SPL = $"Tax for {request.Data.PurchaseOrderItem.Name}";
            purchaseorder.IsTaxEditable = true;
            purchaseorder.CurrencyDate = DateTime.UtcNow;
          
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Closed.Id;
            purchaseorder.PONumber = request.Data.PONumber;

            purchaseorder.PurchaseRequisition = $"Tax for {request.Data.PurchaseOrderItem.Name}";
            purchaseorder.QuoteNo = $"Tax for {request.Data.PurchaseOrderItem.Name}";
            purchaseorder.TaxCode = $"Tax for {request.Data.PurchaseOrderItem.Name}";
         
            purchaseorder.AccountAssigment = request.Data.MWOCECName;
            purchaseorder.MainBudgetItemId = request.Data.MainBudgetItem.BudgetItemId;
            purchaseorder.PurchaseOrderCurrency = request.Data.PurchaseOrderCurrency.Id;
            await Repository.AddPurchaseOrder(purchaseorder);
            var purchaseorderitem = purchaseorder.AddPurchaseOrderItem(request.Data.PurchaseOrderItem.BudgetItemId);
            purchaseorderitem.Name = request.Data.PurchaseOrderItem.Name;
            purchaseorderitem.UnitaryValueCurrency = request.Data.PurchaseOrderItem.QuoteCurrencyValue;
            purchaseorderitem.Quantity = 1;
     
            await Repository.AddPurchaseorderItem(purchaseorderitem);

            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
    
            if (result > 0)
                return Result.Success($"Purchase order created succesfully");
            return Result.Fail($"Purchase order was not created succesfully");
        }
    }
}
