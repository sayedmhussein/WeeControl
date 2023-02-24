using WeeControl.Host.WebApiService;

namespace WeeControl.Host.Test.ApiService.Internals;

public class ApplicationPagesTests
{
    [Fact]
    public void WhenGettingListOfPages_ShouldHaveValue()
    {
        var list = ApplicationPages.Elevator.GetListOfPages();
        
        Assert.NotEmpty(list);
    }
}