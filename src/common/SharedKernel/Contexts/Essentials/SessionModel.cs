using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.SharedKernel.Contexts.Essentials;

public class SessionModel : IEntityModel
{
    [Required]
    public DateTime CreatedTs { get; set; }
    
    public DateTime? TerminationTs { get; set; }
}