using WeeControl.Core.SharedKernel.Services;

namespace WeeControl.Core.Test.SharedKernel;

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
    
    [Fact]
    public void CitiesListShouldNotBeEmptyOrNull()
    {
        var service = new ConstantValueService();

        var cities = service.Countries.First().Cities;

        Assert.NotNull(cities);
        Assert.NotEmpty(cities);
    }
}