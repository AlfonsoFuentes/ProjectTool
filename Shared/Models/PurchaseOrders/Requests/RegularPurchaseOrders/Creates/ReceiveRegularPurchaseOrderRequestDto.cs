using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;

namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates
{
    public class ReceiveRegularPurchaseOrderRequestDto
    {

        public Guid PurchaseOrderId { get; set; }
        public string PONumber { get; set; } = string.Empty;

        public bool IsAssetProductive { get; set; }

        public bool IsAlteration { get; set; }

        public double PercentageAlteration {  get; set; }
        public List<ReceivePurchaseorderItemRequestDto> PurchaseOrderItemsToReceive { get; set; } = new();

        public double SumPOActualUSD => PurchaseOrderItemsToReceive.Count == 0 ? 0 : PurchaseOrderItemsToReceive.Sum(x => x.POActualUSD);
        public double SumPOPendingUSD => PurchaseOrderItemsToReceive.Count == 0 ? 0 : PurchaseOrderItemsToReceive.Sum(x => x.POPendingUSD);

        public void ConvertToDto(ReceiveRegularPurchaseOrderRequest request)
        {
            this.PurchaseOrderId = request.PurchaseOrderId;
            this.PONumber = request.PONumber;
            this.IsAlteration = request.IsAlteration;
            this.IsAssetProductive = request.IsAssetProductive;
            this.PercentageAlteration=request.PercentageAlteration;
            foreach (var item in request.PurchaseOrderItemsToReceive)
            {
                PurchaseOrderItemsToReceive.Add(new()
                {
                    BudgetItemId = item.BudgetItemId,
                    PurchaseOrderItemId = item.PurchaseOrderItemId,
                    POActualUSD = item.PONewActualUSD,
                    POPendingUSD = item.PONewPendingUSD,

                });


            }

        }
    }
}
