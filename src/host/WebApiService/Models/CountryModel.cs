using System.ComponentModel.DataAnnotations;

namespace WeeControl.Host.WebApiService.Models;

public class CountryModel
{
    public string Name { get; init; }

    [StringLength(2, MinimumLength = 2)] public string Code2 { get; init; }

    [StringLength(3, MinimumLength = 3)] public string Code3 { get; init; }

    [Range(1, 999)] public int Numeric { get; init; }

    public string Local { get; }

    public IEnumerable<CityModel> Cities { get; init; }
}