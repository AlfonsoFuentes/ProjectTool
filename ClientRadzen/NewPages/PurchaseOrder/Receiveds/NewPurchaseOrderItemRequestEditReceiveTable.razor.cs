using ClientRadzen.NewPages.PurchaseOrder.Approveds;
using Shared.ExtensionsMetods;
using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.PurchaseOrders.Request;
using System.Collections.Generic;
using System.Linq;

namespace ClientRadzen.NewPages.PurchaseOrder.Receiveds;
#nullable disable
public partial class NewPurchaseOrderItemRequestEditReceiveTable
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public NewPurchaseOrderEditReceivePage MainPage { get; set; }

    NewPurchaseOrderEditReceiveRequest Model => MainPage.Model;
    int MaxColumn = 12;
    RadzenDataGrid<NewPurchaseOrderReceiveItemActualRequest> ordersGrid = null!;
    Density Density = Density.Compact;

   
    [Parameter]
    [EditorRequired]
    public EventCallback<Task<bool>> ValidateAsync { get; set; }

   
  
    async Task OnKeyDownCurrency(KeyboardEventArgs arg, NewPurchaseOrderReceiveItemActualRequest order)
    {
        if (arg.Key == "Enter")
        {
            await ordersGrid.UpdateRow(order);
        }
    }
    async Task EditRow(DataGridRowMouseEventArgs<NewPurchaseOrderReceiveItemActualRequest> order)
    {

        await ordersGrid.EditRow(order.Data);

    }

    async Task ClickCell(DataGridCellMouseEventArgs<NewPurchaseOrderReceiveItemActualRequest> order)
    {

        var column = order.Column;

        if (order.Column.Property == "BudgetItemName")
        {

            await ordersGrid.EditRow(order.Data);

            if (ValidateAsync.HasDelegate)
                await ValidateAsync.InvokeAsync();
        }


    }
    async Task EditRowButton(NewPurchaseOrderReceiveItemActualRequest order)
    {

        await ordersGrid.EditRow(order);
    }
    
    async Task SaveRow(NewPurchaseOrderReceiveItemActualRequest order)
    {
        await ordersGrid.UpdateRow(order);
    }

    void CancelEdit(NewPurchaseOrderReceiveItemActualRequest order)
    {


        ordersGrid.CancelEditRow(order);


    }

   

}
