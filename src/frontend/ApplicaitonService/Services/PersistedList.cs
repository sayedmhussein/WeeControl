using WeeControl.User.UserApplication.Interfaces;
using WeeControl.User.UserApplication.Models;

namespace WeeControl.User.UserApplication.Services;

public class PersistedList : IPersistedLists
{
    public IEnumerable<CountryModel> Countries => new List<CountryModel>()
    {
        new ("USA", "United States", "United States", new []{ new CityModel("CAI", "Cairo", "القاهرة")}),
        new ("EGP", "Egypt", "مصر" , new []{ new CityModel("CAI", "Cairo", "القاهرة")}),
        new ("SAU", "Saudi", "السعودية", new []{ new CityModel("CAI", "Cairo", "القاهرة")})
    };
}