using WeeControl.Host.WebApiService.Contexts.Constants;

namespace WeeControl.Host.WebApiService.Internals.Services;

internal class ConstantValueService : IConstantValue
{
    public IEnumerable<CountryModel> Countries => new List<CountryModel>()
    {
        new ("USA", "United States", "United States", new []{ new CityModel("CAI", "Cairo", "القاهرة")}),
        new ("EGP", "Egypt", "مصر" , new []{ new CityModel("CAI", "Cairo", "القاهرة")}),
        new ("SAU", "Saudi", "السعودية", new []{ new CityModel("CAI", "Cairo", "القاهرة")})
    };
}