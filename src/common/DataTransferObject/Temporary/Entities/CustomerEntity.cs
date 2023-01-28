using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.DataTransferObject.Temporary.Entities;

public class CustomerEntity
{
    [StringLength(3, MinimumLength = 3)]
    public string CountryCode { get; set; } = string.Empty;
}