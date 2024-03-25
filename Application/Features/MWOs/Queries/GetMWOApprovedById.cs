using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Commons.UserManagement;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.CostCenter;
using Shared.Models.MWO;
using Shared.Models.MWOStatus;
using Shared.Models.MWOTypes;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
using System.Linq.Expressions;

namespace Application.Features.MWOs.Queries
{
    public record GetMWOApprovedById(Guid MWOId) : IRequest<IResult<MWOApprovedResponse>>;
    public class GetMWOApprovedByIdHandler : IRequestHandler<GetMWOApprovedById, IResult<MWOApprovedResponse>>
    {
        private IMWORepository Repository { get; set; }

        public GetMWOApprovedByIdHandler(IMWORepository repository)
        {
            Repository = repository;

        }


        public async Task<IResult<MWOApprovedResponse>> Handle(GetMWOApprovedById request, CancellationToken cancellationToken)
        {

            var mwo = await Repository.GetMWOById(request.MWOId);
            if (mwo == null) Result<MWOApprovedResponse>.Fail("MWO Not found");
            MWOApprovedResponse mworesponse = new MWOApprovedResponse()
            {
                Id = mwo!.Id,
                Name = mwo.Name,
                CECName = $"CEC0000{mwo.MWONumber}",
                CostCenter = CostCenterEnum.GetName(mwo.CostCenter),
                CreatedBy = mwo!.CreatedBy,
                IsAssetProductive = mwo.IsAssetProductive,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo!.PercentageContingency,
                PercentageEngineering = mwo!.PercentageEngineering,
                PercentageTaxForAlterations = mwo!.PercentageTaxForAlterations,
                Status = MWOStatusEnum.GetType(mwo.Status),
                MWOType = MWOTypeEnum.GetType(mwo.Type),


            };

            var purchaseorders = await Repository.GetPurchaseOrdersByMWOId(request.MWOId);
            Expression<Func<PurchaseOrder, PurchaseOrderResponse>> expressionpurchaseorder = e => new PurchaseOrderResponse
            {
                MWOId = e.MWOId,
                MWOName = mworesponse.Name,
                PONumber = e.PONumber,
                AccountAssigment = e.AccountAssigment,
                CreatedBy = e.CreatedBy,
                CreatedOn = e.CreatedDate.ToShortDateString(),
                ExpetedOn = e.POExpectedDateDate == null ? string.Empty : e.POExpectedDateDate!.Value.ToShortDateString(),
                IsAlteration = e.IsAlteration,
                IsCapitalizedSalary = e.IsCapitalizedSalary,
                IsTaxEditable = e.IsTaxEditable,
                PurchaseOrderId = e.Id,
                PurchaseorderName = e.PurchaseorderName,
                PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(e.PurchaseOrderStatus),
                PurchaseRequisition = e.PurchaseRequisition,
                QuoteNo = e.QuoteNo,
                Supplier = e.Supplier == null ? string.Empty : e.Supplier.NickName,
                TaxCode = e.TaxCode,
                VendorCode = e.Supplier == null ? string.Empty : e.Supplier.VendorCode,

                PurchaseOrderItems = e.PurchaseOrderItems.Select(x => new PurchaseorderItemsResponse
                {
                    BudgetItemId = x.BudgetItemId,
                    Actual = x.Actual,
                    POValueUSD = e.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id ? 0 : x.POValueUSD,
                    PurchaseorderItemId = x.Id,
                    Potencial = e.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id ? x.POValueUSD : 0,
                }
               ).ToList(),
            };

            mworesponse.PurchaseOrders = purchaseorders.Select(expressionpurchaseorder).ToList();

            var budgetitems = await Repository.GetBudgetItemsByMWOId(request.MWOId);
            Expression<Func<BudgetItem, BudgetItemApprovedResponse>> expression = e => new BudgetItemApprovedResponse
            {

                BudgetItemId = e.Id,
                Name = e.Name,
                Order = e.Order,
                Type = BudgetItemTypeEnum.GetType(e.Type),
                MWOId = e.MWOId,
                MWOCECName = mworesponse.CECName,
                CostCenter = mworesponse.CostCenter,
                MWOName = mworesponse.Name,
                Budget = e.Budget,
                CreatedBy = e.CreatedByUserName,
                CreatedOn = e.CreatedDate.ToString(),
                IsMainItemTaxesNoProductive = e.IsMainItemTaxesNoProductive,
                Quantity = e.Quantity,

                Percentage = e.Percentage,
                Brand = e.Brand == null ? string.Empty : e.Brand.Name,


            };
            var budgetitemresponse = budgetitems.Select(expression).ToList();
            mworesponse.BudgetItems = budgetitemresponse.OrderBy(x => x.Nomenclatore).ToList();
            mworesponse.BudgetItems.ForEach(budgetitem =>
            {
                budgetitem.PurchaseOrders = mworesponse.PurchaseOrders.Where(x => x.PurchaseOrderItems.Any(x => x.BudgetItemId == budgetitem.BudgetItemId)).ToList();
            });

            return Result<MWOApprovedResponse>.Success(mworesponse);
        }
    }

}
