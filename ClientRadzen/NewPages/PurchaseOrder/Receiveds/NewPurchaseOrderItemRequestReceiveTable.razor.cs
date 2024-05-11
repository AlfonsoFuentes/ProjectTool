using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.PurchaseOrders.Request;

namespace ClientRadzen.NewPages.PurchaseOrder.Receiveds;
#nullable disable
public partial class NewPurchaseOrderItemRequestReceiveTable
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public NewPurchaseOrderReceivePage MainPage { get; set; }

    OldPurchaseOrderReceiveRequest Model => MainPage.Model;
    int MaxColumn = 12;
    RadzenDataGrid<NewPurchaseOrderReceiveItemRequest> ordersGrid = null!;
    Density Density = Density.Compact;

    async Task EditRow(DataGridRowMouseEventArgs<NewPurchaseOrderReceiveItemRequest> order)
    {

        await ordersGrid.EditRow(order.Data);

    }
    [Parameter]
    [EditorRequired]
    public EventCallback<Task<bool>> ValidateAsync { get; set; }
  
   

    async Task ClickCell(DataGridCellMouseEventArgs<NewPurchaseOrderReceiveItemRequest> order)
    {
       
        await ordersGrid.EditRow(order.Data);

        if (ValidateAsync.HasDelegate)
            await ValidateAsync.InvokeAsync();


    }
    NewBudgetItemToCreatePurchaseOrderResponse ItemToAdd;
    



    async Task OnKeyDownCurrency(KeyboardEventArgs arg, NewPurchaseOrderReceiveItemRequest order)
    {
        if (arg.Key == "Enter")
        {
            await ordersGrid.UpdateRow(order);
        }
    }

    async Task EditRowButton(NewPurchaseOrderReceiveItemRequest order)
    {

        await ordersGrid.EditRow(order);
    }
    
    async Task SaveRow(NewPurchaseOrderReceiveItemRequest order)
    {
        await ordersGrid.UpdateRow(order);
    }

    void CancelEdit(NewPurchaseOrderReceiveItemRequest order)
    {


        ordersGrid.CancelEditRow(order);


    }
   
}
