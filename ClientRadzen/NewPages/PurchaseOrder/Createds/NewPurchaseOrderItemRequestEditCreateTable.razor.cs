using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.PurchaseOrders.Request;
using System.Collections.Generic;
using System.Linq;

namespace ClientRadzen.NewPages.PurchaseOrder.Createds;
#nullable disable
public partial class NewPurchaseOrderItemRequestEditCreateTable
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public NewPurchaseOrderEditCreatePage MainPage { get; set; }

    NewPurchaseOrderCreateEditRequest Model => MainPage.Model;
    int MaxColumn = 12;
    RadzenDataGrid<NewPurchaseOrderCreateItemRequest> ordersGrid = null!;
    Density Density = Density.Compact;

    
    [Parameter]
    [EditorRequired]
    public EventCallback<Task<bool>> ValidateAsync { get; set; }

  
    NewBudgetItemToCreatePurchaseOrderResponse ItemToAdd;
    async Task AddNewItem(NewPurchaseOrderCreateItemRequest order)
    {
        MainPage.AddBudgetItemToPurchaseOrder(ItemToAdd);

        await ordersGrid.UpdateRow(order);
        await ordersGrid.Reload();
        if (ValidateAsync.HasDelegate)
            await ValidateAsync.InvokeAsync();
        ItemToAdd = null!;
    }



    async Task OnKeyDownCurrency(KeyboardEventArgs arg, NewPurchaseOrderCreateItemRequest order)
    {
        if (arg.Key == "Enter")
        {
            await ordersGrid.UpdateRow(order);
        }
    }
    async Task EditRow(DataGridRowMouseEventArgs<NewPurchaseOrderCreateItemRequest> order)
    {

        await ordersGrid.EditRow(order.Data);

    }
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
    async Task EditRowButton(NewPurchaseOrderCreateItemRequest order)
    {

        await ordersGrid.EditRow(order);
    }
    async Task DeleteRow(NewPurchaseOrderCreateItemRequest order)
    {


        if (Model.PurchaseOrderItems.Contains(order) && order.BudgetItemId != Model.MainBudgetItemId)
        {

            MainPage.RemovedBudgetItemToPurchaseOrder(order);


            await ordersGrid.Reload();
            if (ValidateAsync.HasDelegate)
                await ValidateAsync.InvokeAsync();
        }
        else
        {
            ordersGrid.CancelEditRow(order);
            await ordersGrid.Reload();
        }
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
