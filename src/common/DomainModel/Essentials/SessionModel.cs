using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.DomainModel.Essentials;

public class SessionModel : IEntityModel
{
    [Required] public DateTime CreatedTs { get; set; } = DateTime.UtcNow;

    public DateTime? TerminationTs { get; set; }
}