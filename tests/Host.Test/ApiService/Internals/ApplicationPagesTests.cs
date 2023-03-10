using WeeControl.Host.WebApiService.Data;

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