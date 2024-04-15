﻿using Application.Features.PurchaseOrders.Validators.RegularPurchaseOrders;
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
            purchaseorder.USDCOP = request.Data.USDCOP;
            purchaseorder.SPL = $"Tax for {request.Data.PurchaseOrderItem.Name}";
            purchaseorder.IsTaxEditable = true;
            purchaseorder.CurrencyDate = DateTime.UtcNow;
            purchaseorder.POValueCurrency = request.Data.SumPOValueCurrency;
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Closed.Id;
            purchaseorder.PONumber = request.Data.PONumber;
            purchaseorder.ActualCurrency = request.Data.SumPOValueCurrency;
            purchaseorder.QuoteNo = $"Tax for {request.Data.PurchaseOrderItem.Name}";
            purchaseorder.TaxCode = $"Tax for {request.Data.PurchaseOrderItem.Name}";
            purchaseorder.POClosedDate = DateTime.UtcNow;
            purchaseorder.AccountAssigment = request.Data.MWOCECName;
            purchaseorder.MainBudgetItemId = request.Data.MainBudgetItem.BudgetItemId;
            purchaseorder.Currency = request.Data.PurchaseOrderCurrency.Id;
            await Repository.AddPurchaseOrder(purchaseorder);
            var purchaseorderitem = purchaseorder.AddPurchaseOrderItem(request.Data.PurchaseOrderItem.BudgetItemId, request.Data.PurchaseOrderItem.Name);
            purchaseorderitem.UnitaryValueCurrency = request.Data.PurchaseOrderItem.CurrencyUnitaryValue;
            purchaseorderitem.Quantity = 1;
            purchaseorderitem.ActualCurrency = request.Data.PurchaseOrderItem.TotalCurrencyValue;
            await Repository.AddPurchaseorderItem(purchaseorderitem);

            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
        
            if (result > 0)
                return Result.Success($"Purchase order created succesfully");
            return Result.Fail($"Purchase order was not created succesfully");
        }
    }
}
