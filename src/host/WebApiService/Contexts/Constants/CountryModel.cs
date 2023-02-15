using System.ComponentModel.DataAnnotations;

namespace WeeControl.Host.WebApiService.Contexts.Constants;

public class CountryModel
{
    [Key]
    [StringLength(3, MinimumLength = 3)]
    public string CountryCode { get; }
    public string CountryName { get; }
    public string LocalName { get; }

    public IEnumerable<CityModel> Cities { get; }

    public CountryModel(string code, string name, string localName, IEnumerable<CityModel> cities)
    {
        CountryCode = code;
        CountryName = name;
        LocalName = localName;
        Cities = cities;
    }
}