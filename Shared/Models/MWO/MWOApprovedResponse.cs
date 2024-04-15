using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.MWOStatus;
using Shared.Models.MWOTypes;
using Shared.Models.PurchaseOrders.Responses;

namespace Shared.Models.MWO
{

    public class MWOApprovedResponse
    {
        public Guid Id { get; set; }
        public string CECMWOName => $"{CECName}-{Name}";
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum MWOType { get; set; } = MWOTypeEnum.None;
        public string Type => MWOType.Name;
        public MWOStatusEnum Status { get; set; } = MWOStatusEnum.None;
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
       
        public string CECName { get; set; } = string.Empty;
        public string CostCenter { get; set; } = string.Empty;

       
        public double Appropiation => Expenses + Capital;
        public double Expenses => BudgetItemsAlterations.Sum(x => x.Budget);
        public double Capital => BudgetItemsCapital.Sum(x => x.Budget);
        public double ActualExpenses => PurchaseOrderAlterations.Sum(x => x.Actual);
        public double AssignedExpenses => PurchaseOrderAlterations.Sum(x=>x.Assigned);
        public double PotencialExpenses => PurchaseOrderAlterations.Sum(x => x.Potencial);
        public double CommitmentExpenses => PurchaseOrderAlterations.Sum(x => x.Commitment);
        public double PendingExpenses => Expenses - AssignedExpenses - PotencialExpenses;


        
        public double PotencialCapital => PurchaseOrderCapital.Sum(x => x.Potencial);
        public double AssignedCapital => PurchaseOrderCapital.Sum(x => x.Assigned);
        public double ActualCapital => PurchaseOrderCapital.Sum(x => x.Actual);
        public double CommitmentCapital => PurchaseOrderCapital.Sum(x => x.Commitment);
        public double PendingCapital => Capital - AssignedCapital - PotencialCapital;
        public List<BudgetItemApprovedResponse> BudgetItems { get; set; } = new();      

        public bool IsAssetProductive { get; set; } = true;
     
        public double PercentageAssetNoProductive { get; set; } = 19;
        public double PercentageEngineering { get; set; } = 6;
        public double PercentageContingency { get; set; } = 10;
        public double PercentageTaxForAlterations { get; set; } = 19;
        public bool HasExpenses => BudgetItemsAlterations.Count > 0;
        public List<BudgetItemApprovedResponse> BudgetItemsAlterations => BudgetItems.Where(x => x.Type.Id == BudgetItemTypeEnum.Alterations.Id).ToList();

        public List<BudgetItemApprovedResponse> BudgetItemsCapital => BudgetItems.Where(x => x.Type.Id != BudgetItemTypeEnum.Alterations.Id).ToList();
        public List<PurchaseOrderResponse> PurchaseOrders { get; set; } = new();
        public List<PurchaseOrderResponse> PurchaseOrderAlterations => PurchaseOrders.Where(x => x.IsAlteration==true).ToList();

        public List<PurchaseOrderResponse> PurchaseOrderCapital => PurchaseOrders.Where(x => x.IsAlteration == false).ToList();
    }
}
