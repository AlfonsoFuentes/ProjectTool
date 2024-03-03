using Shared.Models.BudgetItemTypes;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;

namespace Shared.Models.BudgetItems
{
    public class BudgetItemApprovedResponse
    {
        public ListBudgetItemResponse Parent { get; set; } = new();
        public List<string> ValidationErrors { get; set; } = new();
        public Guid MWOId { get; set; }
        public string MWOName { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public string Nomenclatore { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ComposedName => (Type.Id == BudgetItemTypeEnum.Equipments.Id || Type.Id == BudgetItemTypeEnum.Instruments.Id) ?
                Brand != string.Empty ? $"{Name} {Brand} Qty: {Quantity}" : $"{Name} Qty: {Quantity}" :
                Type.Id == BudgetItemTypeEnum.Contingency.Id ? $"{Name} {Percentage}%" :
                Type.Id == BudgetItemTypeEnum.Engineering.Id ? Percentage > 0 ? $"{Name} {Percentage}%" :
                Name :
                Type.Id == BudgetItemTypeEnum.Taxes.Id ? $"{Name} {Percentage}%" :
                Name;

        public List<PurchaseOrderItemForBudgetItemResponse> PurchaseOrders { get; set; } = new();
        public string NomenclatoreName => $"{Nomenclatore} - {Name}";
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;
        public bool IsNotAbleToEditDelete { get; set; }
        public bool IsMainItemTaxesNoProductive { get; set; }

        public double Budget { get; set; }
        public double Assigned => PurchaseOrders.Count == 0 ? 0 : PurchaseOrders.Where(x => x.PurchaseOrderStatus.Id != PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD);
        public double PotencialAssigned => PurchaseOrders.Count == 0 ? 0 : PurchaseOrders.Where(x => x.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD);
        public double Actual => PurchaseOrders.Count == 0 ? 0 : PurchaseOrders.Sum(x => x.Actual);
        public double Commitment => Assigned - Actual;
        public double Pending => Budget - Assigned - PotencialAssigned;
        public int Order { get; set; }
        public bool Existing { get; set; }
        public double Quantity { get; set; }
        public void ChangeQuantity(string quantitystring)
        {
            ValidationErrors.Clear();
            double quantity = 0;
            if (!double.TryParse(quantitystring, out quantity))
                return;

            Quantity = quantity;
            Budget = Quantity * UnitaryCost;
        }
        public double UnitaryCost { get; set; }
        public void ChangeUnitaryCost(string UnitaryCoststring)
        {
            ValidationErrors.Clear();
            double unitarycost = 0;
            if (!double.TryParse(UnitaryCoststring, out unitarycost))
                return;

            UnitaryCost = unitarycost;
            Budget = Quantity * UnitaryCost;
        }
        public string? Brand { get; set; }

        public bool MWOApproved { get; set; }
        public string? Model { get; set; } = string.Empty;
        public string? Reference { get; set; } = string.Empty;
        public double Percentage { get; set; }
        public void ChangePercentage(string Percentagestring)
        {
            ValidationErrors.Clear();
            double percentage = 0;
            if (!double.TryParse(Percentagestring, out percentage))
                return;
            SumPercentage -= Percentage;
            Percentage = percentage;
            SumPercentage += Percentage;
            if (Type.Id == BudgetItemTypeEnum.Taxes.Id)
            {
                Budget = SumBudgetTaxes * Percentage / 100.0;
            }
            else
            {
                Budget = SumBudget * Percentage / (100 - SumPercentage);
            }

        }
        public double SumPercentage { get; set; }
        public double SumBudget { get; set; }
        public double SumBudgetTaxes { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;


    }
}
