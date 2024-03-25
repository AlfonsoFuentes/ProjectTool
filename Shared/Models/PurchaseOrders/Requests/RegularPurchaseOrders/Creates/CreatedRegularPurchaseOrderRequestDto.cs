using Shared.Models.Currencies;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.Suppliers;

namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates
{
    public class CreatedRegularPurchaseOrderRequestDto
    {
        public CreatedRegularPurchaseOrderRequestDto()
        {


        }
        public void ConvertToDto(CreatedRegularPurchaseOrderRequest request)
        {
            this.MainBudgetItemId = request.MainBudgetItemId;
            this.SupplierId = request.Supplier!.Id;
            this.VendorCode = request.VendorCode;
            this.TaxCode = request.TaxCode;
            this.PurchaseorderName = request.PurchaseorderName;
            this.QuoteNo = request.QuoteNo;
            this.PurchaseRequisition = request.PurchaseRequisition;



            this.USDCOP = request.TRMUSDCOP;
            this.USDEUR = request.TRMUSDEUR;
            this.IsMWONoProductive = request.IsMWONoProductive;
            this.IsAlteration = request.IsAlteration;
            this.AccountAssigment = request.AccountAssignment;
            this.SPL = request.SPL;
            this.PurchaseOrderCurrency = request.PurchaseOrderCurrency.Id;
            this.QuoteCurrency = request.QuoteCurrency.Id;
            this.MWOId = request.MWOId;
            this.CurrencyDate = request.CurrencyDate;
            request.PurchaseOrderItemNoBlank.ForEach(row =>
            {
                PurchaseOrderItemRequestDto dto = new();
                dto.ConvertToDto(row);
                PurchaseOrderItems.Add(dto);

            });
           
        }
        public DateTime CurrencyDate { get; set; }
        public Guid MWOId { get; set; } = new();
        public Guid MainBudgetItemId { get; set; }
        public Guid SupplierId { get; set; }
        public string VendorCode { get; set; } = string.Empty;
        public string TaxCode { get; set; } = string.Empty;
        public string PurchaseorderName { get; set; } = string.Empty;
        public string QuoteNo { get; set; } = string.Empty;
        public string PurchaseRequisition { get; set; } = string.Empty;
        public List<PurchaseOrderItemRequestDto> PurchaseOrderItems { get; set; } = new();
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public bool IsMWONoProductive { get; set; }
        public bool IsAlteration { get; set; }
        public string AccountAssigment { get; set; } = string.Empty;
        public string SPL { get; set; } = string.Empty;
        public int PurchaseOrderCurrency { get; set; }
        public int QuoteCurrency { get; set; }





    }

}
