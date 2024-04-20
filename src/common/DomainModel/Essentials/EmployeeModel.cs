using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.DomainModel.Essentials;

public class EmployeeModel : IValidatableModel
{
    [Required] [StringLength(45)] public string EmployeeNo { get; set; } = string.Empty;

    [NotMapped] public string SupervisorEmployeeNo { get; set; } = string.Empty;
}