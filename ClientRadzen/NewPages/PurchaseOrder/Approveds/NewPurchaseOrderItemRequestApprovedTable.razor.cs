using ClientRadzen.NewPages.PurchaseOrder.Approveds;
using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.PurchaseOrders.Request;
using System.Collections.Generic;
using System.Linq;

namespace ClientRadzen.NewPages.PurchaseOrder.Approveds;
#nullable disable
public partial class NewPurchaseOrderItemRequestApprovedTable
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public NewPurchaseOrderApprovedPage MainPage { get; set; }

    NewPurchaseOrderApproveRequest Model => MainPage.Model;
    int MaxColumn = 12;
    RadzenDataGrid<NewPurchaseOrderCreateItemRequest> ordersGrid = null!;
    Density Density = Density.Compact;

    async Task EditRow(DataGridRowMouseEventArgs<NewPurchaseOrderCreateItemRequest> order)
    {

        await ordersGrid.EditRow(order.Data);

    }
    [Parameter]
    [EditorRequired]
    public EventCallback<Task<bool>> ValidateAsync { get; set; }


    async Task ClickCell(DataGridCellMouseEventArgs<NewPurchaseOrderCreateItemRequest> order)
    {
        var column = order.Column;
        var row = order.Data.BudgetItemId == Guid.Empty;
        if (order.Column.Property == "BudgetItemName" && order.Data.BudgetItemId == Guid.Empty)
        {

            await ordersGrid.EditRow(order.Data);

            if (ValidateAsync.HasDelegate)
                await ValidateAsync.InvokeAsync();
        }


    }
    NewBudgetItemToCreatePurchaseOrderResponse ItemToAdd;
    



    async Task OnKeyDownCurrency(KeyboardEventArgs arg, NewPurchaseOrderCreateItemRequest order)
    {
        if (arg.Key == "Enter")
        {
            await ordersGrid.UpdateRow(order);
        }
    }

    async Task EditRowButton(NewPurchaseOrderCreateItemRequest order)
    {

        await ordersGrid.EditRow(order);
    }
    
    async Task SaveRow(NewPurchaseOrderCreateItemRequest order)
    {
        await ordersGrid.UpdateRow(order);
    }

    void CancelEdit(NewPurchaseOrderCreateItemRequest order)
    {


        ordersGrid.CancelEditRow(order);


    }
}
