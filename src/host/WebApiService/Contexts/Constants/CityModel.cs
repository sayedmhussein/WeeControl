using System.ComponentModel.DataAnnotations;

namespace WeeControl.Host.WebApiService.Contexts.Constants;

public class CityModel
{
    [Key]
    public string CityCode { get; }
    public string CityName { get; }
    public string LocalName { get; }

    public CityModel(string code, string name, string localName)
    {
        CityCode = code;
        CityName = name;
        LocalName = localName;
    }
}