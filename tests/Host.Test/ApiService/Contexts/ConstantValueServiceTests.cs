
using WeeControl.Host.WebApiService.Internals.Services;

namespace WeeControl.Host.Test.ApiService.Contexts;

public class ConstantValueServiceTests
{
    [Fact]
    public void CountriesListShouldNotBeEmptyOrNull()
    {
        var service = new ConstantValueService();

        var countries = service.Countries;

        Assert.NotNull(countries);
        Assert.NotEmpty(countries);
    }
}