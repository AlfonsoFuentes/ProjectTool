#nullable disable

using ClientRadzen.Enums;
using Shared.NewModels.EBPReport;
using Shared.NewModels.PurchaseOrders.Responses;

namespace ClientRadzen.Pages.SapAdjust;
public partial class NewMWOEBPSapAdjustReport
{
    [CascadingParameter]
    private App MainApp { get; set; }

    [Parameter]
    [EditorRequired]
    public NewEBPReportResponse EBPReport { get; set; }
    PurchaseorderView View;
    public List<NewPriorPurchaseOrderResponse> PurchaseOrders =>
        View == PurchaseorderView.None ? new() :
        View == PurchaseorderView.Actual ? EBPReport.ActualPurchaseOrders :
        View == PurchaseorderView.Commitment ? EBPReport.CommitmentPurchaseOrders :
        EBPReport.PotentialPurchaseOrders;



    void CellClick(DataGridCellMouseEventArgs<NewSummaryTotalResponse> cell)
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
