using System.ComponentModel.DataAnnotations;

namespace WeeControl.Common.SharedKernel.Contexts.Essential.Entities;

public class EmployeeEntity
{
    [Required]
    [StringLength(45)]
    public string EmployeeNo { get; set; } = string.Empty;
}