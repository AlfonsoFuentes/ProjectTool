using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Commands.RegularPurchaseOrders.Creates
{
    public record CreatePurchaseOrderCapitalizedSalaryCommand(CreateCapitalizedSalaryPurchaseOrderRequest Data) : IRequest<IResult>;
    internal class CreatePurchaseOrderCapitalizedSalaryCommandHandler : IRequestHandler<CreatePurchaseOrderCapitalizedSalaryCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        private IMWORepository MWORepository { get; set; }
        public CreatePurchaseOrderCapitalizedSalaryCommandHandler(IAppDbContext appDbContext, IPurchaseOrderRepository repository, IMWORepository mWORepository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(CreatePurchaseOrderCapitalizedSalaryCommand request, CancellationToken cancellationToken)
        {
            
            var mwo = await Repository.GetMWOWithBudgetItemsAndPurchaseOrderById(request.Data.MWOId);
            if (mwo == null)
            {
                return Result.Fail($"MWO Not found");

            }
            var purchaseorder = mwo.AddPurchaseOrder();
            purchaseorder.PurchaseorderName = request.Data.PurchaseOrderName;
            purchaseorder.PurchaseRequisition = request.Data.PurchaseorderNumber;
            purchaseorder.PONumber = request.Data.PurchaseorderNumber;
            purchaseorder.QuoteCurrency = CurrencyEnum.USD.Id;
            purchaseorder.IsAlteration = false;
            purchaseorder.USDEUR = request.Data.USDEUR;
            purchaseorder.USDCOP = request.Data.USDCOP;
            purchaseorder.SPL = "";
            purchaseorder.IsCapitalizedSalary = true;
            purchaseorder.CurrencyDate = DateTime.UtcNow;
            purchaseorder.POValueCurrency = request.Data.SumPOValueUSD;
            purchaseorder.ActualCurrency = request.Data.SumPOValueUSD;
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Closed.Id;
            purchaseorder.QuoteNo = "";
            purchaseorder.TaxCode = "";
            purchaseorder.POClosedDate=DateTime.UtcNow;
            purchaseorder.AccountAssigment = request.Data.MWOCECName;
            purchaseorder.MainBudgetItemId = request.Data.MainBudgetItemId;
            purchaseorder.Currency = CurrencyEnum.USD.Id;
            await Repository.AddPurchaseOrder(purchaseorder);
            var purchaseorderitem = purchaseorder.AddPurchaseOrderItem(purchaseorder.MainBudgetItemId,request.Data.PurchaseOrderItem.Name);
            purchaseorderitem.UnitaryValueCurrency = request.Data.PurchaseOrderItem.POValueUSD;
            purchaseorderitem.Quantity = 1;
            purchaseorderitem.ActualCurrency = request.Data.SumPOValueUSD;
            await Repository.AddPurchaseorderItem(purchaseorderitem);

            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            //await MWORepository.UpdateDataForApprovedMWO(purchaseorder.MWOId, cancellationToken);
            if (result > 0)
                return Result.Success($"Purchase order created succesfully");
            return Result.Fail($"Purchase order was not created succesfully");
        }
    }
}
