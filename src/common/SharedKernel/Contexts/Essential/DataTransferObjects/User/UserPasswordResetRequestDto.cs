using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Contexts.Essential.Entities;

namespace WeeControl.SharedKernel.Contexts.Essential.DataTransferObjects.User;

public class UserPasswordResetRequestDto
{
    public static UserPasswordResetRequestDto Create(string email, string username)
    {
        return new UserPasswordResetRequestDto()
        {
            Email = email, Username = username
        };
    }
    
    [Required]
    [StringLength(255)] 
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500, MinimumLength = 3)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}