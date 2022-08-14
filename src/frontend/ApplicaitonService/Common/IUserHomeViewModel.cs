using WeeControl.Frontend.ApplicationService.Customer.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;

namespace WeeControl.Frontend.ApplicationService.Common;

public interface IUserHomeViewModel : IViewModelBase
{
    string GreetingMessage { get; }
    Task Init();
    
    List<MenuItemModel> MenuItems { get; }
    Task ChangeMyPasswordAsync();
}