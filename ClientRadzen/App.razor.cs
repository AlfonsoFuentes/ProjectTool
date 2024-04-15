using Client.Infrastructure.Managers.ChangeUser;
using Client.Infrastructure.Managers.CurrencyApis;
using Microsoft.AspNetCore.Components;
using Radzen;
#nullable disable
namespace ClientRadzen;
public partial class App
{
    [Inject]
    public IRate _CurrencyService { get; set; }
    public void ShowTooltip(ElementReference elementReference, string text, TooltipPosition position) =>
       TooltipService.Open(elementReference, text, new TooltipOptions() { Position = position });

    public DateOnly CurrentDate => DateOnly.FromDateTime(DateTime.Now);
    public ConversionRate RateList { get; set; }

    public double USDCOP => RateList == null ? 0 : Math.Round(RateList.COP, 2);
    public double USDEUR => RateList == null ? 0 : Math.Round(RateList.EUR, 2);

    public string USDCOPLabel => String.Format(new System.Globalization.CultureInfo("en-US"), "{0:C0}", USDCOP);
    public string USDEURLabel => String.Format(new System.Globalization.CultureInfo("en-US"), "{0:C0}", USDEUR);
   
    protected override async Task OnInitializedAsync()
    {
        RateList = await _CurrencyService.GetRates();

       


    }
}
