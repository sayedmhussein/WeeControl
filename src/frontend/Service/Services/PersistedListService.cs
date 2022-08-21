using WeeControl.Frontend.Service.Contexts.Essential.Models;
using WeeControl.Frontend.Service.Interfaces;

namespace WeeControl.Frontend.Service.Services;

public class PersistedListService : IPersistedLists
{
    public IEnumerable<CountryModel> Countries => new List<CountryModel>()
    {
        new ("USA", "United States", "United States", new []{ new CityModel("CAI", "Cairo", "القاهرة")}),
        new ("EGP", "Egypt", "مصر" , new []{ new CityModel("CAI", "Cairo", "القاهرة")}),
        new ("SAU", "Saudi", "السعودية", new []{ new CityModel("CAI", "Cairo", "القاهرة")})
    };
}