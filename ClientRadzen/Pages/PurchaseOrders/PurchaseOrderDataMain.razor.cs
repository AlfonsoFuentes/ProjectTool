#nullable disable
using Shared.Models.PurchaseOrders.Responses;

namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class PurchaseOrderDataMain
    {
        [CascadingParameter]
        public App MainApp { get; set; }
        void ChangeIndex(int index)
        {
            MainApp.TabIndexPurchaseOrder = index;
        }
        [Inject]
        public IPurchaseOrderService Service { get; set; } = null!;

        public IEnumerable<NewPurchaseOrderCreatedResponse> PurchaseordersCreated => Response.PurchaseordersCreated;
        public IEnumerable<NewPurchaseOrderApprovedResponse> PurchaseordersToReceive => Response.PurchaseordersApproved;
        public IEnumerable<NewPurchaseOrderClosedResponse> PurchaseordersClosed => Response.PurchaseordersClosed;

        NewPurchaseOrdersListResponse Response = new();

        protected override async Task OnInitializedAsync()
        {
            await UpdateAll();
           

        }
        public async Task UpdateAll()
        {
            var result = await Service.GetAllPurchaseOrders();
            if (result.Succeeded)
            {
                Response = result.Data;

            }

        }
    }
}
