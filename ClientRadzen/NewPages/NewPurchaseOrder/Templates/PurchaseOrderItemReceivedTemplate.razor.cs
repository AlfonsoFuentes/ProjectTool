using ClientRadzen.NewPages.NewPurchaseOrder.Createds;
using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.PurchaseOrders.Base;
using Shared.NewModels.PurchaseOrders.Request;
#nullable disable

namespace ClientRadzen.NewPages.NewPurchaseOrder.Templates;
public partial class PurchaseOrderItemReceivedTemplate
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public PurchaseOrderTemplate MainPage { get; set; }

    
    int MaxColumn = 12;
    RadzenDataGrid<NewPurchaseOrderItemReceivedRequest> ordersGrid = null!;
    Density Density = Density.Compact;
    NewBudgetItemMWOApprovedResponse ItemToAdd;
    


    async Task OnKeyDownCurrency(KeyboardEventArgs arg, NewPurchaseOrderItemReceivedRequest order)
    {
        if (arg.Key == "Enter")
        {
            await ordersGrid.UpdateRow(order);
        }
    }
    async Task EditRow(DataGridRowMouseEventArgs<NewPurchaseOrderItemReceivedRequest> order)
    {

        await ordersGrid.EditRow(order.Data);

    }
    async Task ClickCell(DataGridCellMouseEventArgs<NewPurchaseOrderItemReceivedRequest> order)
    {

        await ordersGrid.EditRow(order.Data);

        await MainPage.ValidateAsync();


    }
    async Task EditRowButton(NewPurchaseOrderItemReceivedRequest order)
    {

        await ordersGrid.EditRow(order);
    }
  
    async Task SaveRow(NewPurchaseOrderItemReceivedRequest order)
    {
        await ordersGrid.UpdateRow(order);
    }

    void CancelEdit(NewPurchaseOrderItemReceivedRequest order)
    {


        ordersGrid.CancelEditRow(order);


    }

}
