using WeeControl.Frontend.ApplicationService.Services;

namespace WeeControl.Frontend.ApplicationService.UnitTest.Services;

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