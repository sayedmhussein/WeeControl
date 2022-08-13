namespace WeeControl.Frontend.ApplicationService.Customer.Models;

public class MenuItemModel
{
    public static MenuItemModel Create(string name)
    {
        return new MenuItemModel() { Name = name, PageName = name};
    }
        
    public string? Name { get; private init; }
    public string? PageName { get; private init; }

    private MenuItemModel()
    {
    }
}