﻿using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Commands.RegularPurchaseOrders.Edits
{
    public record EditPurchaseOrderClosedCommand(EditPurchaseOrderRegularClosedRequest Data) : IRequest<IResult>;
    internal class EditPurchaseOrderClosedCommandHandler : IRequestHandler<EditPurchaseOrderClosedCommand, IResult>
    {
        private IPurchaseOrderRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }
        private IMWORepository MWORepository { get; set; }
        public EditPurchaseOrderClosedCommandHandler(IPurchaseOrderRepository repository, IAppDbContext appDbContext, IMWORepository mWORepository)
        {
            Repository = repository;
            AppDbContext = appDbContext;
            MWORepository = mWORepository;
        }

        public async Task<IResult> Handle(EditPurchaseOrderClosedCommand request, CancellationToken cancellationToken)
        {

            var purchaseOrder = await Repository.GetPurchaseOrderById(request.Data.PurchaseOrderId);

            if (purchaseOrder == null)
            {
                return Result.Fail($"Purchase order :{request.Data.PurchaseorderName} Not found");
            }
            purchaseOrder.PONumber = request.Data.PONumber;
            purchaseOrder.POExpectedDateDate = request.Data.ExpectedDate;
            purchaseOrder.PurchaseorderName = request.Data.PurchaseorderName;
            purchaseOrder.QuoteNo = request.Data.QuoteNo;
            purchaseOrder.SupplierId = request.Data.SupplierId;
            purchaseOrder.QuoteCurrency = request.Data.QuoteCurrency.Id;
            purchaseOrder.USDCOP = request.Data.USDCOP;
            purchaseOrder.USDEUR = request.Data.USDEUR;
            purchaseOrder.MainBudgetItemId = request.Data.MainBudgetItemId;
            double poactualCurrency = purchaseOrder.ActualCurrency;
            var newValueCurrency = request.Data.PurchaseOrderItemNoBlank.Sum(x => x.TotalCurrencyValue);

            if (newValueCurrency > poactualCurrency)
            {
                purchaseOrder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Receiving.Id;

            }
            else
            {
                purchaseOrder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Closed.Id;
                purchaseOrder.ActualCurrency = newValueCurrency;

            }


            foreach (var item in request.Data.PurchaseOrderItemNoBlank)
            {
                var purchaseorderItem = await Repository.GetPurchaseOrderItemById(item.PurchaseOrderItemId);
                if (purchaseorderItem == null)
                {
                    purchaseorderItem = purchaseOrder.AddPurchaseOrderItem(item.BudgetItemId, item.Name);
                    purchaseorderItem.UnitaryValueCurrency = item.CurrencyUnitaryValue;
                    purchaseorderItem.Quantity = item.Quantity;
                    await Repository.AddPurchaseorderItem(purchaseorderItem);
                    if (purchaseOrder.IsAlteration)
                    {
                        var purchaseordertaxestem = purchaseOrder.AddPurchaseOrderItemForAlteration(item.BudgetItemId,
                   $"{request.Data.PONumber} Tax {item.BudgetItemName} {purchaseOrder.MWO.PercentageTaxForAlterations}%");
                        purchaseordertaxestem.UnitaryValueCurrency = purchaseOrder.MWO.PercentageTaxForAlterations / 100.0 * item.TotalCurrencyValue;
                        purchaseordertaxestem.Quantity = 1;

                        await Repository.AddPurchaseorderItem(purchaseordertaxestem);

                    }
                    await AppDbContext.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    purchaseorderItem.Name = item.Name;
                    purchaseorderItem.Quantity = item.Quantity;
                    purchaseorderItem.UnitaryValueCurrency = item.CurrencyUnitaryValue;
                    purchaseorderItem.ActualCurrency = newValueCurrency < purchaseorderItem.ActualCurrency ? newValueCurrency : purchaseorderItem.ActualCurrency;
                    if (item.BudgetItemId != purchaseorderItem.BudgetItemId)
                        purchaseorderItem.ChangeBudgetItem(item.BudgetItemId);
                    await Repository.UpdatePurchaseOrderItem(purchaseorderItem);
                }

            }
            var sumPOValueCurrency = request.Data.PurchaseOrderItems.Count == 0 ? 0 :
                  request.Data.PurchaseOrderItems.Sum(x => x.TotalCurrencyValue);

            if (purchaseOrder.IsAlteration)
            {
                foreach (var row in request.Data.PurchaseOrderItemNoBlank)
                {
                    var alterationsitem = await Repository.GetPurchaseOrderItemsAlterationsById(purchaseOrder.Id, row.BudgetItemId);
                    if (alterationsitem != null)
                    {
                        var currencyvalue = row.TotalCurrencyValue * purchaseOrder.MWO.PercentageTaxForAlterations / 100;
                        alterationsitem.UnitaryValueCurrency = currencyvalue;
                        alterationsitem.Quantity = 1;
                        await Repository.UpdatePurchaseOrderItem(alterationsitem);
                        sumPOValueCurrency += currencyvalue;
                    }

                }

            }
            foreach (var row in purchaseOrder.PurchaseOrderItems)
            {
                if (!request.Data.PurchaseOrderItems.Any(x => x.BudgetItemId == row.BudgetItemId))
                {
                    AppDbContext.PurchaseOrderItems.Remove(row);
                }
            }
            if (request.Data.IsMWONoProductive && !request.Data.IsAlteration)
            {
                var TaxBudgetitem = await Repository.GetTaxBudgetItemNoProductive(purchaseOrder.MWOId);
                if (TaxBudgetitem != null)
                {

                    var purchaseordertaxestem = await Repository.GetPurchaseOrderMainTaxItemById(TaxBudgetitem.Id, purchaseOrder.MWOId);

                    purchaseordertaxestem.UnitaryValueCurrency = TaxBudgetitem.Percentage / 100.0 * purchaseOrder.POValueCurrency;
                    await Repository.UpdatePurchaseOrderItem(purchaseordertaxestem);

                }

            }
            purchaseOrder.POValueCurrency = sumPOValueCurrency;

            await Repository.UpdatePurchaseOrder(purchaseOrder);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            //await MWORepository.UpdateDataForApprovedMWO(purchaseOrder.MWOId, cancellationToken);
            if (result > 0)
            {
                return Result.Success($"Purchase order :{request.Data.PurchaseorderName} was edited succesfully");
            }

            return Result.Fail($"Purchase order :{request.Data.PurchaseorderName} was not edited succesfully");
        }
    }
}
