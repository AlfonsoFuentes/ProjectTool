using Microsoft.AspNetCore.Components;
using Shared.Models.PurchaseOrders.Responses;
#nullable disable
namespace ClientRadzen.Pages.MWOPages.Templates;
public partial class TemplatePurchaseOrderItemResponse
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public List<NewPurchaseOrderItemResponse> PurchaseOrderItems { get; set; } = new();
    [Parameter]
    public bool IsMainItemTaxesNoProductive { get; set; } = false;
    [CascadingParameter]
    public MWOApproved MWOApproved { get; set; }

}
