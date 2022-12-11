using WeeControl.Frontend.AppService.Contexts.Essential.Models;
using WeeControl.Frontend.AppService.Interfaces;

namespace WeeControl.Frontend.AppService.Services;

public class PersistedListService : IPersistedLists
{
    public IEnumerable<CountryModel> Countries => new List<CountryModel>()
    {
        new ("USA", "United States", "United States", new []{ new CityModel("CAI", "Cairo", "القاهرة")}),
        new ("EGP", "Egypt", "مصر" , new []{ new CityModel("CAI", "Cairo", "القاهرة")}),
        new ("SAU", "Saudi", "السعودية", new []{ new CityModel("CAI", "Cairo", "القاهرة")})
    };
}