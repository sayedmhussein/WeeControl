using System.ComponentModel.DataAnnotations;

namespace WeeControl.Frontend.AppService.Models;

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