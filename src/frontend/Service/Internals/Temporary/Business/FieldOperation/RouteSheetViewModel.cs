using System.Collections.ObjectModel;
using WeeControl.Core.DataTransferObject.Contexts.Business.FieldOperation;

namespace WeeControl.Frontend.AppService.Internals.Temporary.Business.FieldOperation;

public class RouteSheetViewModel
{
    private ObservableCollection<RouteSheetDto> RouteSheet { get; }

    private string searchText = string.Empty;
    public string SearchText
    {
        get => searchText;
        set
        {
            searchText = value;

        }
    }

    public RouteSheetViewModel()
    {
        RouteSheet = new ObservableCollection<RouteSheetDto>();
    }
}