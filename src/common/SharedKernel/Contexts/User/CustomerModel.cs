using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.SharedKernel.Contexts.User;

public class CustomerModel
{
    [StringLength(3, MinimumLength = 3)]
    public string CountryCode { get; set; } = string.Empty;
}