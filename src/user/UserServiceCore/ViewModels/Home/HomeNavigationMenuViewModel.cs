using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.Security;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.ViewModels.Home;

public class HomeNavigationMenuViewModel : ViewModelBase
{
    private readonly IDevice device;

    public IEnumerable<MenuItem> MenuItems { get; private set; } = new List<MenuItem>();

    public HomeNavigationMenuViewModel(IDevice device)
    {
        this.device = device;
    }

    public async Task SetupMenuAsync()
    {
        var list = new List<MenuItem>();
        
        foreach (var claim in await device.Security.GetClaimsAsync())
        {
            if (claim.Type == ClaimsTagsList.Claims.Session)
            {
                continue;
            }

            if (ClaimsTagsList.GetClaimsDictionary().ContainsValue(claim.Type))
            {
                var name = ClaimsTagsList.GetClaimsDictionary().First(x => x.Value == claim.Type);
                list.Add(MenuItem.Create(name.Key));
            }
        }

        MenuItems = new List<MenuItem>(list);
    }
    
    public class MenuItem
    {
        public static MenuItem Create(string name)
        {
            return new MenuItem(){ Name=name, PageName = name};
        }
        
        public string Name { get; private init; }
        public string PageName { get; private init; }

        private MenuItem()
        {
        }
    }
}