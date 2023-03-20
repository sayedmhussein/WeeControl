using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.SharedKernel.Models;

public class CityModel
{
    public string Name { get; init; }
    
    [StringLength(3, MinimumLength = 3)]
    public string Code3 { get; init; }
    
    public string Local { get; init; }
}