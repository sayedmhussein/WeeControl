using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.SharedKernel.Contexts.User;

public class AddressModel : IEntityModel
{
    [StringLength(25)]
    public string Line1 { get; set; } = string.Empty;

    [StringLength(25)]
    public string Line2 { get; set; } = string.Empty;

    [StringLength(5)]
    public string ZipCode { get; set; } = string.Empty;

    [StringLength(25)]
    public string City { get; set; } = string.Empty;

    [StringLength(3, MinimumLength = 3)]
    public string CountryCode { get; set; } = string.Empty;

    [Range(-180, 180)]
    public double? Latitude { get; set; }
    
    [Range(-90, 90)]
    public double? Longitude { get; set; }
}