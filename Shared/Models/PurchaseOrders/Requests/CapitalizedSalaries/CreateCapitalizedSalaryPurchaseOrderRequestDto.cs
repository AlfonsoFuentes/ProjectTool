using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;

namespace Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries
{
    public class CreateCapitalizedSalaryPurchaseOrderRequestDto
    {


        public void ConverToDto(CreateCapitalizedSalaryPurchaseOrderRequest request)
        {
           
            this.USDCOP = request.TRMUSDCOP;
            this.USDEUR = request.TRMUSDEUR;
            this.SumPOValueUSD = request.SumPOValueUSD;
          
            this.IsCapitalizedSalary = request.IsCapitalizedSalary;
            this.MainBudgetItemId = request.MainBudgetItem.BudgetItemId;
            this.MWOId = request.MWOId;
            this.PurchaseOrderCurrency = request.PurchaseOrderCurrency.Id;
            this.PurchaseOrderName = request.PurchaseOrderName;
            this.PurchaseorderNumber = request.PurchaseorderNumber;
            this.PurchaseOrderItem = request.PurchaseOrderItem;
            
          this.MWOCECName = request.MWOCECName;

        }
     
        public bool IsCapitalizedSalary { get; set; } = true;

        public string MWOCECName { get; set; } = string.Empty;
        public string PurchaseOrderName { get; set; } = string.Empty;
        public string PurchaseorderNumber { get; set; } = string.Empty;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }

        public int PurchaseOrderCurrency { get; set; }
        public PurchaseOrderItemRequest PurchaseOrderItem { get; set; } = new();
        
        public Guid MWOId { get; set; }
    
        public Guid MainBudgetItemId { get; set; } = new();
       
        public double SumPOValueUSD { get; set; }
       


    }
}
