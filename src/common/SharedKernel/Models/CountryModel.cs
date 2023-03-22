using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.SharedKernel.Models;

public class CountryModel
{
    public string Name { get; init; } = string.Empty;
    
    [StringLength(2, MinimumLength = 2)]
    public string Code2 { get; init; } = string.Empty;
    
    [StringLength(3, MinimumLength = 3)]
    public string Code3 { get; init; } = string.Empty;
    
    [Range(1, 999)]
    public int Numeric { get; init; } 
    
    public string Local { get; } = string.Empty;

    public IEnumerable<CityModel> Cities { get; init; }
}