using Shared.Models.BudgetItemTypes;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;

namespace Shared.Models.BudgetItems
{
    public class BudgetItemResponse
    {
        public Guid MWOId { get; set; }
        public Guid Id { get; set; }
        public string Nomenclatore { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public List<PurchaseOrderItemForBudgetItemResponse> PurchaseOrders { get; set; } = new();
        public string NomenclatoreName => $"{Nomenclatore} - {Name}";
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;
        public bool IsNotAbleToEditDelete { get; set; }
        public bool IsMainItemTaxesNoProductive { get; set; }
        public double UnitaryCost { get; set; }
        public double Budget { get; set; }
        public double Assigned => PurchaseOrders.Count == 0 ? 0 : PurchaseOrders.Where(x => x.PurchaseOrderStatus.Id != PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD);
        public double PotencialAssigned => PurchaseOrders.Count == 0 ? 0 : PurchaseOrders.Where(x => x.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD);
        public double Actual => PurchaseOrders.Count == 0 ? 0 : PurchaseOrders.Sum(x => x.Actual);
        public double Commitment => Assigned - Actual;
        public double Pending => Budget - Assigned - PotencialAssigned;
        public int Order { get; set; }
        public bool Existing { get; set; }
        public double Quantity { get; set; }
        public string? Brand { get; set; }

        public bool MWOApproved { get; set; }
        public string? Model { get; set; } = string.Empty;
        public string? Reference { get; set; } = string.Empty;
        public double Percentage { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
    }
}
