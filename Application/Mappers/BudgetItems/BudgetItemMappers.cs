

using Application.Mappers.PurchaseOrders;

namespace Application.Mappers.BudgetItems
{
    public static class BudgetItemMappers
    {
        public static NewBudgetItemMWOCreatedResponse ToBudgetItemMWOCreatedResponse(this BudgetItem budgetitem)
        {
            return new()
            {
                Brand = budgetitem.Brand == null ? null : new()
                {
                    BrandId = budgetitem.Brand.Id,
                    Name = budgetitem.Brand.Name,

                },
                BudgetItemId = budgetitem.Id,
                MWOId = budgetitem.MWOId,
                MWOName = budgetitem.MWO == null ? string.Empty : budgetitem.MWO.Name,
                Existing = budgetitem.Existing,
                IsMainItemTaxesNoProductive = budgetitem.IsMainItemTaxesNoProductive,
                IsNotAbleToEditDelete = budgetitem.IsNotAbleToEditDelete,
                Name = budgetitem.Name,
                Order = budgetitem.Order,
                Quantity = budgetitem.Quantity,
                UnitaryCost = budgetitem.UnitaryCost,
                Type = BudgetItemTypeEnum.GetType(budgetitem.Type),
                Percentage = budgetitem.Percentage,
                IsAssetProductive = budgetitem.MWO == null ? false : budgetitem.MWO.IsAssetProductive,



            };
        }
        public static void FromBudgetItemCreateRequest(this NewBudgetItemMWOCreatedRequest request, BudgetItem budgetitem)
        {

            budgetitem.Name = request.Name;

            budgetitem.Quantity = request.Quantity;
            budgetitem.UnitaryCost = request.UnitaryCost;

            budgetitem.BrandId = request.Brand == null ? null : request.BrandId;
            budgetitem.Model = request.Model;
            budgetitem.Reference = request.Reference;
            budgetitem.Percentage = request.Percentage;

        }
        public static void FromBudgetItemUpdateRequest(this NewBudgetItemMWOUpdateRequest request, BudgetItem budgetitem)
        {

            budgetitem.Name = request.Name;

            budgetitem.Quantity = request.Quantity;
            budgetitem.UnitaryCost = request.UnitaryCost;

            budgetitem.BrandId = request.Brand == null ? null : request.BrandId;
            budgetitem.Model = request.Model;
            budgetitem.Reference = request.Reference;
            budgetitem.Percentage = request.Percentage;

        }

        public static NewBudgetItemMWOUpdateRequest ToBudgetItemMWOUpdateRequest(this BudgetItem budgetitem)
        {
            return new()
            {
                Brand = budgetitem.Brand == null ? null : new()
                {
                    BrandId = budgetitem.Brand.Id,
                    Name = budgetitem.Brand.Name,

                },
                BudgetItemId = budgetitem.Id,
                MWOId = budgetitem.MWOId,
                Existing = budgetitem.Existing,
                IsMainItemTaxesNoProductive = budgetitem.IsMainItemTaxesNoProductive,
                IsNotAbleToEditDelete = budgetitem.IsNotAbleToEditDelete,
                IsEngineeringItem = budgetitem.IsEngineeringItem,
                Name = budgetitem.Name,
                Order = budgetitem.Order,
                Quantity = budgetitem.Quantity,
                UnitaryCost = budgetitem.UnitaryCost,
                Type = BudgetItemTypeEnum.GetType(budgetitem.Type),
                Percentage = budgetitem.Percentage,
                IsAssetProductive = budgetitem.MWO == null ? false : budgetitem.MWO.IsAssetProductive,
                Model = budgetitem.Model,
                MWOName = budgetitem.MWO == null ? string.Empty : budgetitem.MWO.CECName,
                Reference = budgetitem.Reference,
                TaxesSelectedItems = (budgetitem.TaxesItems == null || budgetitem.TaxesItems.Count == 0) ? new() :
                budgetitem.TaxesItems.Select(x => x.Selected.ToBudgetItemMWOCreatedResponse()).ToList(),


            };
        }

        public static NewBudgetItemMWOApprovedResponse ToBudgetItemMWOApprovedResponse(this BudgetItem budgetItem)
        {
            return new()
            {
                BudgetItemId = budgetItem.Id,
                Brand = budgetItem.Brand == null ? null : new()
                {
                    BrandId = budgetItem.Brand.Id,
                    Name = budgetItem.Brand.Name,

                },
                Model = budgetItem.Model,
                IsNotAbleToEditDelete = budgetItem.IsNotAbleToEditDelete,
                IsMainItemTaxesNoProductive = budgetItem.IsMainItemTaxesNoProductive,
                IsEngineeringItem = budgetItem.IsEngineeringItem,
                Existing = budgetItem.Existing,
                MWOId = budgetItem.MWOId,
                MWOName = budgetItem.MWOName,
                MWOCECName = budgetItem.CECName,
                MWOCostCenter = budgetItem.CostCenter,
                Name = budgetItem.Name,
                Order = budgetItem.Order,
                Percentage = budgetItem.Percentage,
                Quantity = budgetItem.Quantity,
                Reference = budgetItem.Reference,
                Type = BudgetItemTypeEnum.GetType(budgetItem.Type),
                UnitaryCostUSD = budgetItem.UnitaryCost,
                PurchaseOrderItems = budgetItem.PurchaseOrderItems.Where(x => x.IsTaxAlteration == false).Select(x => x.ToPurchaseOrderItemResponse()).ToList(),


            };
        }
        public static NewBudgetItemToCreatePurchaseOrderResponse ToBudgetItemToCreatePurchaseOrder(this BudgetItem budgetItem)
        {
            return new()
            {
                BudgetItemId = budgetItem.Id,

                IsNotAbleToEditDelete = budgetItem.IsNotAbleToEditDelete,
                IsMainItemTaxesNoProductive = budgetItem.IsMainItemTaxesNoProductive,
                IsEngineeringItem = budgetItem.IsEngineeringItem,

                MWOId = budgetItem.MWOId,
                MWOName = budgetItem.MWOName,
                MWOCECName = budgetItem.CECName,
                MWOCostCenter = budgetItem.CostCenter.Name,
                Name = budgetItem.Name,
                Order = budgetItem.Order,

                Quantity = budgetItem.Quantity,

                Type = BudgetItemTypeEnum.GetType(budgetItem.Type),
                UnitaryCostUSD = budgetItem.UnitaryCost,
                PurchaseOrderItems = budgetItem.PurchaseOrderItems.Where(x => x.IsTaxAlteration == false).Select(x => x.ToPurchaseOrderItemResponse()).ToList(),

            };
        }

        public static NewBudgetItemMWOApprovedResponse ToBudgetItemMWOApproved(this BudgetItem budgetItem)
        {
            return new()
            {
                BudgetItemId = budgetItem.Id,

                IsNotAbleToEditDelete = budgetItem.IsNotAbleToEditDelete,
                IsMainItemTaxesNoProductive = budgetItem.IsMainItemTaxesNoProductive,
                IsEngineeringItem = budgetItem.IsEngineeringItem,

                MWOId = budgetItem.MWOId,
                MWOName = budgetItem.MWOName,
                MWOCECName = budgetItem.CECName,
                MWOCostCenter = budgetItem.CostCenter,
                MWOFocus = budgetItem.Focus,
                MWOIsAssetProductive = budgetItem.IsAssetProductive,
                MWOStatus = budgetItem.MWOStatus,
                MWOType = budgetItem.MWOType,
                Name = budgetItem.Name,
                Order = budgetItem.Order,
                Quantity = budgetItem.Quantity,
                Type = BudgetItemTypeEnum.GetType(budgetItem.Type),
                UnitaryCostUSD = budgetItem.UnitaryCost,
                PurchaseOrderItems = budgetItem.PurchaseOrderItems.Where(x => x.IsTaxAlteration == false).Select(x => x.ToPurchaseOrderItemResponse()).ToList(),

            };
        }
    }
}
