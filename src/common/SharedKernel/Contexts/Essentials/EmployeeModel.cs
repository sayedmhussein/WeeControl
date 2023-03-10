using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.SharedKernel.Contexts.Essentials;

public class EmployeeModel : IEntityModel
{
    [Required] [StringLength(45)] public string EmployeeNo { get; set; } = string.Empty;

    [NotMapped] public string SupervisorEmployeeNo { get; set; } = string.Empty;
}