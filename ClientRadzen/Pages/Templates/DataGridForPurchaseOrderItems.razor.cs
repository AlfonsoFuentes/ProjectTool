using Radzen.Blazor;
using Radzen;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Microsoft.AspNetCore.Components;
using Shared.Models.BudgetItems;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;
using Microsoft.AspNetCore.Components.Web;
using Shared.Models.PurchaseorderStatus;
#nullable disable
namespace ClientRadzen.Pages.Templates;
public partial class DataGridForPurchaseOrderItems
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public PurchaseOrderRequest Model { get; set; }
    int MaxColumn = 12;
    RadzenDataGrid<PurchaseOrderItemRequest> ordersGrid = null!;
    Density Density = Density.Compact;
    [Parameter]
    [EditorRequired]
    public List<BudgetItemApprovedResponse> BudgetItems {  get; set; }= new();
    [Parameter]
    [EditorRequired]
    public List<BudgetItemApprovedResponse> OriginalBudgetItems { get; set; } = new();
    async Task EditRow(DataGridRowMouseEventArgs<PurchaseOrderItemRequest> order)
    {

        await ordersGrid.EditRow(order.Data);

    }
    [Parameter]
    [EditorRequired]
    public EventCallback<Task<bool>> ValidateAsync { get; set; }

   
    async Task ClickCell(DataGridCellMouseEventArgs<PurchaseOrderItemRequest> order)
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
    BudgetItemApprovedResponse ItemToAdd;
    async Task AddNewItem(PurchaseOrderItemRequest order)
    {
        AddBudgetItem(ItemToAdd!);


        if (BudgetItems.Count > 0)
        {
            AddBlankItem();
        }
        await ordersGrid.UpdateRow(order);
        await ordersGrid.Reload();
        if (ValidateAsync.HasDelegate)
            await ValidateAsync.InvokeAsync();
        ItemToAdd = null!;
    }

    public void AddBlankItem()
    {
        if (!Model.PurchaseOrderItems.Any(x => x.BudgetItemId == Guid.Empty))
        {
            Model.PurchaseOrderItems.Add(new PurchaseOrderItemRequest());
        }


    }
    public void AddBudgetItem(BudgetItemApprovedResponse response)
    {
        if (Model.PurchaseOrderItems.Count == 0)
            Model.PurchaseOrderItems.Add(new PurchaseOrderItemRequest());

        Model.PurchaseOrderItems[Model.PurchaseOrderItems.Count - 1].SetBudgetItem(response, Model.USDCOP, Model.USDEUR);


    }
    public async Task ChangeName(PurchaseOrderItemRequest model, string name)
    {

        model.Name = name;

        if (Model.PurchaseOrderItemNoBlank.Count == 1)
        {
            Model.PurchaseorderName = name;
        }
        if (ValidateAsync.HasDelegate)
            await ValidateAsync.InvokeAsync();
    }
    public async Task ChangeQuantity(PurchaseOrderItemRequest item, string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double quantity = item.Quantity;
        if (!double.TryParse(arg, out quantity))
        {

        }
        item.Quantity = quantity;
        if(Model.PurchaseOrderStatus.Id==PurchaseOrderStatusEnum.Created.Id)
        {
            item.PotencialCurrency = item.OriginalPotencialCurrency + item.TotalValuePurchaseOrderCurrency;
        }
        else
        {
            item.AssignedCurrency = item.TotalValuePurchaseOrderCurrency;
        }
        
        if (ValidateAsync.HasDelegate)
            await ValidateAsync.InvokeAsync();
    }
    async Task OnKeyDownCurrency(KeyboardEventArgs arg, PurchaseOrderItemRequest order)
    {
        if (arg.Key == "Enter")
        {
            await ordersGrid.UpdateRow(order);
        }
    }
    public async Task ChangeCurrencyValue(PurchaseOrderItemRequest item, string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double currencyvalue = item.Quantity;
        if (!double.TryParse(arg, out currencyvalue))
        {

        }
        item.QuoteCurrencyValue = currencyvalue;
        if (Model.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id)
        {
            item.PotencialCurrency = item.OriginalPotencialCurrency + item.TotalValuePurchaseOrderCurrency;
        }
        else
        {
            item.AssignedCurrency = item.TotalValuePurchaseOrderCurrency;
        }
        if (ValidateAsync.HasDelegate)
            await ValidateAsync.InvokeAsync();
    }
    async Task EditRowButton(PurchaseOrderItemRequest order)
    {

        await ordersGrid.EditRow(order);
    }
    async Task DeleteRow(PurchaseOrderItemRequest order)
    {


        if (Model.PurchaseOrderItems.Contains(order) && order.BudgetItemId != Model.MainBudgetItemId)
        {

            Model.PurchaseOrderItems.Remove(order);
            var datatoadd = OriginalBudgetItems.Single(x => x.BudgetItemId == order.BudgetItemId);
            BudgetItems.Add(datatoadd);
            if (BudgetItems.Count > 0)
            {
                AddBlankItem();
            }
            await ordersGrid.Reload();
        }
        else
        {
            ordersGrid.CancelEditRow(order);
            await ordersGrid.Reload();
        }
    }
    async Task SaveRow(PurchaseOrderItemRequest order)
    {
        await ordersGrid.UpdateRow(order);
    }

    void CancelEdit(PurchaseOrderItemRequest order)
    {


        ordersGrid.CancelEditRow(order);


    }
}
