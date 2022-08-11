using System.ComponentModel.DataAnnotations;

namespace WeeControl.SharedKernel.Essential.Entities;

public class EmployeeEntity
{
    [Required]
    [StringLength(45)]
    public string EmployeeNo { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255, MinimumLength = 3)]
    public string TerritoryName { get; set; } = string.Empty;
}