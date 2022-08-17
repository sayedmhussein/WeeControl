using System.ComponentModel.DataAnnotations;

namespace WeeControl.Frontend.ApplicationService.Contexts.Essential.Models;

public class CityModel
{
    [Key] 
    public string CityCode { get; init; }
    public string CityName { get; init; }
    public string LocalName { get; init; }

    public CityModel(string code, string name, string localName)
    {
        CityCode = code;
        CityName = name;
        LocalName = localName;
    }
}