using Application.Features.PurchaseOrders.Validators.RegularPurchaseOrders;
using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.Taxes;
using Shared.Models.PurchaseorderStatus;

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
            var validator = new CreateTaxPurchaseOrderValidator(Repository);
            var validationResult = await validator.ValidateAsync(request.Data, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList());

            }
            var mwo = await Repository.GetMWOWithBudgetItemsAndPurchaseOrderById(request.Data.MWOId);
            if (mwo == null)
            {
                return Result.Fail($"MWO Not found");

            }
            var purchaseorder = mwo.AddPurchaseOrder();
            purchaseorder.PurchaseorderName = request.Data.Name;
            purchaseorder.PurchaseRequisition = $"Tax for {request.Data.PurchaseOrderItem.Name}";
            purchaseorder.QuoteCurrency = 0;
            purchaseorder.IsAlteration = false;
            purchaseorder.USDEUR = request.Data.USDEUR;
            purchaseorder.SPL = $"Tax for {request.Data.PurchaseOrderItem.Name}";
            purchaseorder.IsTaxEditable = true;
            purchaseorder.CurrencyDate = DateTime.UtcNow;
            purchaseorder.POValueUSD = request.Data.PurchaseOrderItem.POItemValueUSD;
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Closed.Id;
            purchaseorder.PONumber = request.Data.PONumber;
            purchaseorder.Actual = request.Data.SumPOValueUSD;
            purchaseorder.QuoteNo = $"Tax for {request.Data.PurchaseOrderItem.Name}";
            purchaseorder.TaxCode = $"Tax for {request.Data.PurchaseOrderItem.Name}";
            purchaseorder.USDCOP = request.Data.USDCOP;
            purchaseorder.AccountAssigment = request.Data.MWOCECName;
            purchaseorder.MainBudgetItemId = request.Data.MainBudgetItem.BudgetItemId;
            purchaseorder.Currency = request.Data.PurchaseOrderCurrency.Id;
            await Repository.AddPurchaseOrder(purchaseorder);
            var purchaseorderitem = purchaseorder.AddPurchaseOrderItem(request.Data.PurchaseOrderItem.BudgetItemId, request.Data.PurchaseOrderItem.Name);
            purchaseorderitem.POValueUSD = request.Data.PurchaseOrderItem.POItemValueUSD;
            purchaseorderitem.Quantity = 1;
            purchaseorderitem.Actual = request.Data.PurchaseOrderItem.UnitaryCostInUSD;
            await Repository.AddPurchaseorderItem(purchaseorderitem);

            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            await MWORepository.UpdateDataForApprovedMWO(purchaseorder.MWOId, cancellationToken);
            if (result > 0)
                return Result.Success($"Purchase order created succesfully");
            return Result.Fail($"Purchase order was not created succesfully");
        }
    }
}
