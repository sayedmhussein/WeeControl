using System.ComponentModel.DataAnnotations;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.DomainModel.Essentials.Dto;

public class UserPasswordResetRequestDto : IEntityModel
{
    [Required] [StringLength(255)] public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(500, MinimumLength = 3)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public static UserPasswordResetRequestDto Create(string email, string username)
    {
        return new UserPasswordResetRequestDto
        {
            Email = email,
            Username = username
        };
    }
}