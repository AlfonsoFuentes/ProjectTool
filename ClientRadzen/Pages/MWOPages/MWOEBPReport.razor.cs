using ClientRadzen.Pages.Enums;
#nullable disable
namespace ClientRadzen.Pages.MWOPages;
public partial class MWOEBPReport
{
    [Parameter]
    public MWOEBPResponse MWOEBPResponse { get; set; } = new();
    [CascadingParameter]
    public App MainApp { get; set; }
    PurchaseorderView View;
 
    void CellClick(DataGridCellMouseEventArgs<SummaryTotal> cell)
    {
        var colum = cell.Column.Property;
        if (colum.Contains("Actual") && View != PurchaseorderView.Actual)
        {
            View = PurchaseorderView.Actual;

        }

        else if (colum.Contains("Commitment") && View != PurchaseorderView.Commitment)
        {
            View = PurchaseorderView.Commitment;

        }
        else if (colum.Contains("Potential") && View != PurchaseorderView.Potential)
        {
            View = PurchaseorderView.Potential;

        }
        else
        {
            View = PurchaseorderView.None;
        }
        StateHasChanged();
    }
   
}
