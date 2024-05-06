using Azure;
using Shared.NewModels.BudgetItems.Responses;
using System.Linq;

namespace ClientRadzen.NewPages.MWOS.Tables;
#nullable disable
public partial class NewMWOCreatedTableList
{
    [CascadingParameter]
    private App MainApp { get; set; }
    [CascadingParameter]
    private NewMWOMain MainPage { get; set; }

    Func<NewMWOCreatedResponse, bool> fiterexpresion => x =>
       x.Name.Contains(MainPage.nameFilterCreated, StringComparison.CurrentCultureIgnoreCase) ||
       x.Type.Name.Contains(MainPage.nameFilterCreated, StringComparison.CurrentCultureIgnoreCase) ||
       x.Focus.Name.Contains(MainPage.nameFilterCreated, StringComparison.CurrentCultureIgnoreCase)
       ;
    public List<NewMWOCreatedResponse> FilteredItems => MainPage.MWOsCreated.Count == 0 ? new() :
        MainPage.MWOsCreated?.Where(fiterexpresion).ToList();
}
