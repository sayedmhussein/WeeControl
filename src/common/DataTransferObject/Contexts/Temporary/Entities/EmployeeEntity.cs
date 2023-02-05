using System.ComponentModel.DataAnnotations;

namespace WeeControl.Core.DataTransferObject.Contexts.Temporary.Entities;

public class EmployeeEntity
{
    [Required]
    [StringLength(45)]
    public string EmployeeNo { get; set; } = string.Empty;
}