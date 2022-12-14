using System.Collections.ObjectModel;
using WeeControl.Common.SharedKernel.Contexts.Business;
using WeeControl.Common.SharedKernel.Contexts.Business.FieldOperation;

namespace WeeControl.Frontend.AppService.Contexts.Business.FieldOperation;

public class RouteSheetViewModel : ViewModelBase
{
    private ObservableCollection<RouteSheetDto> RouteSheet { get; }

    private string searchText = string.Empty;
    public string SearchText
    {
        get => searchText;
        set
        {
            searchText = value;
            OnPropertyChanged(nameof(SearchText));
        }
    }

    public RouteSheetViewModel()
    {
        RouteSheet = new ObservableCollection<RouteSheetDto>();
    }
}