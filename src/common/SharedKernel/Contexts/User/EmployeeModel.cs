using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.SharedKernel.Contexts.User;

public class EmployeeModel
{
    [Required]
    [StringLength(45)]
    public string EmployeeNo { get; set; } = string.Empty;
}