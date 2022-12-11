using WeeControl.Frontend.AppService.Services;

namespace WeeControl.Frontend.Service.UnitTest.Services;

public class PersistedListServiceTests
{
    [Fact]
    public void CountriesListShouldNotBeEmptyOrNull()
    {
        var service = new PersistedListService();

        var countries = service.Countries;
        
        Assert.NotNull(countries);
        Assert.NotEmpty(countries);
    }
}