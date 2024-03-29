#nullable disable
using Azure;
using Client.Infrastructure.Managers.PurchaseOrders;
using Microsoft.AspNetCore.Components;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;

namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class PurchaseOrderDataMain
    {
        [Inject]
        private IPurchaseOrderService Service { get; set; } = null!;

        public IEnumerable<PurchaseOrderResponse> PurchaseordersCreated => Response.PurchaseordersCreated;
        public IEnumerable<PurchaseOrderResponse> PurchaseordersToReceive => Response.PurchaseordersApproved;
        public IEnumerable<PurchaseOrderResponse> PurchaseordersClosed => Response.PurchaseordersClosed;

        PurchaseOrdersListResponse Response = new();

        protected override async Task OnInitializedAsync()
        {

            var result = await Service.GetAllPurchaseOrders();
            if (result.Succeeded)
            {
                Response = result.Data;

            }

        }
    }
}
