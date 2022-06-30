namespace WeeControl.User.UserApplication.ViewModels.Home;

public class MenuItem
{
    public static MenuItem Create(string name)
    {
        return new MenuItem() { Name = name, PageName = name};
    }
        
    public string? Name { get; private init; }
    public string? PageName { get; private init; }

    private MenuItem()
    {
    }
}