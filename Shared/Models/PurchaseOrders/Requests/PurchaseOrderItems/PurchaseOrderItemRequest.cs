using Shared.Enums.Currencies;
using Shared.Models.BudgetItems;

namespace Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems
{
    public class PurchaseOrderItemRequest
    {
        public PurchaseOrderItemRequest() { }
        public void SetBudgetItem(BudgetItemApprovedResponse _BudgetItem, double usdcop, double usdeur)
        {
            BudgetItemName = _BudgetItem.NomenclatoreName;
            Name = _BudgetItem.Name;
            BudgetItemId = _BudgetItem.BudgetItemId;
            BudgetUSD = _BudgetItem.BudgetUSD;
            TRMUSDCOP = usdcop;
            TRMUSDEUR = usdeur;
            AssignedUSD = _BudgetItem.AssignedUSD;
            PotencialUSD = _BudgetItem.PotentialUSD;
            ApprovedUSD = _BudgetItem.ApprovedUSD;
            OriginalAssignedCurrency = AssignedUSD;
            OriginalPotencialCurrency = PotencialUSD;

        }
        public double OriginalAssignedCurrency { get; set; }
        public double OriginalPotencialCurrency { get; set; }
        public double BudgetUSD { get; set; }
        public double ApprovedUSD { get; set; }
        public double CommitmentUSD => ApprovedUSD - ActualUSD;
        public double AssignedUSD { get; set; }
        
        public double PotencialUSD { get; set; }
        public double NewPotencialUSD => PotencialUSD + PurchaseOrderValueUSD;
        public double ActualUSD { get; set; }
        public Guid PurchaseOrderItemId { get; set; }
        public string Name { get; set; } = string.Empty;

        public Guid BudgetItemId { get; set; } = Guid.Empty;
        public string BudgetItemName { get; set; } = string.Empty;
        public double Quantity { get; set; } = 1;

        public double NewAssignedUSD => AssignedUSD + PurchaseOrderValueUSD;

        public double PendingToCommitmUSD => BudgetUSD - NewAssignedUSD;

        public void ChangeCurrency(CurrencyEnum newCurrency)
        {
            double originalValueInUsd = UnitaryValueFromQuoteUSD;
            if (newCurrency.Id == CurrencyEnum.COP.Id)
            {
                QuoteCurrencyValue = originalValueInUsd * TRMUSDCOP;
            }
            else if (newCurrency.Id == CurrencyEnum.EUR.Id)
            {
                QuoteCurrencyValue = originalValueInUsd * TRMUSDEUR;
            }
            else if (newCurrency.Id == CurrencyEnum.USD.Id)
            {
                QuoteCurrencyValue = originalValueInUsd;
            }
            QuoteCurrency = newCurrency;

        }


        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.COP;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.COP;

        public double TRMUSDCOP { get; set; } = 1;
        public double TRMUSDEUR { get; set; } = 1;

        double _QuoteCurrencyValue { get; set; }
        public double QuoteCurrencyValue
        {
            get
            {
                return _QuoteCurrencyValue;
            }
            set
            {
                _QuoteCurrencyValue = value;


            }
        }

        public double UnitaryValueFromQuoteUSD =>
          QuoteCurrency.Id == CurrencyEnum.USD.Id ? QuoteCurrencyValue :
          QuoteCurrency.Id == CurrencyEnum.COP.Id ? QuoteCurrencyValue / TRMUSDCOP :
          QuoteCurrency.Id == CurrencyEnum.EUR.Id ? QuoteCurrencyValue / TRMUSDEUR :
          0;
        public double PurchaseOrderValueUSD => Quantity * UnitaryValueFromQuoteUSD;

        public double UnitaryValuePurchaseOrderCurrency =>
           PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? UnitaryValueFromQuoteUSD :
           PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? UnitaryValueFromQuoteUSD * TRMUSDCOP :
           PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? UnitaryValueFromQuoteUSD * TRMUSDEUR :
           0;
        public double PurchaseOrderValuePurchaseOrderCurrency => Quantity * UnitaryValuePurchaseOrderCurrency;

      

       



    }

}
