using ClientRadzen.NewPages.NewPurchaseOrder.Createds;
using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.PurchaseOrders.Base;
using Shared.NewModels.PurchaseOrders.Request;
#nullable disable

namespace ClientRadzen.NewPages.NewPurchaseOrder.Templates;
public partial class PurchaseOrderItemTemplate
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public PurchaseOrderTemplate MainPage { get; set; }

    
    int MaxColumn = 12;
    RadzenDataGrid<NewPurchaseOrderItemRequest> ordersGrid = null!;
    Density Density = Density.Compact;
    NewBudgetItemMWOApprovedResponse ItemToAdd;
    async Task AddNewItem(NewPurchaseOrderItemRequest order)
    {
        await MainPage.AddBudgetItemToPurchaseOrder(ItemToAdd);

        await ordersGrid.UpdateRow(order);
        await ordersGrid.Reload();

        ItemToAdd = null!;
    }



    async Task OnKeyDownCurrency(KeyboardEventArgs arg, NewPurchaseOrderItemRequest order)
    {
        if (arg.Key == "Enter")
        {
            await ordersGrid.UpdateRow(order);
        }
    }
    async Task EditRow(DataGridRowMouseEventArgs<NewPurchaseOrderItemRequest> order)
    {
        if (MainPage.IsNotAbleToEditMainData) return;
        await ordersGrid.EditRow(order.Data);

    }
    async Task ClickCell(DataGridCellMouseEventArgs<NewPurchaseOrderItemRequest> order)
    {
        if (MainPage.IsNotAbleToEditMainData) return;
        var column = order.Column;
        var row = order.Data.BudgetItemId == Guid.Empty;
        if (order.Column.Property == "BudgetItemName" && order.Data.BudgetItemId == Guid.Empty)
        {

            await ordersGrid.EditRow(order.Data);

            await MainPage.ValidateAsync();
        }


    }
    async Task EditRowButton(NewPurchaseOrderItemRequest order)
    {

        await ordersGrid.EditRow(order);
    }
    async Task DeleteRow(NewPurchaseOrderItemRequest order)
    {


        if (MainPage.Model.PurchaseOrderItems.Contains(order) && order.BudgetItemId != MainPage.Model.MainBudgetItemId)
        {

            await MainPage.RemovedBudgetItemToPurchaseOrder(order);
            await ordersGrid.Reload();
            await MainPage.ValidateAsync();
        }
        else
        {
            ordersGrid.CancelEditRow(order);
            await ordersGrid.Reload();
        }
    }
    async Task SaveRow(NewPurchaseOrderItemRequest order)
    {
        await ordersGrid.UpdateRow(order);
    }

    void CancelEdit(NewPurchaseOrderItemRequest order)
    {


        ordersGrid.CancelEditRow(order);


    }

}
