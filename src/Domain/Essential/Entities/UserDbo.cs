using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // --> 88NE5286 Rami-Khambasi <--

namespace WeeControl.Domain.Essential.Entities;

public class UserDbo
{
    public static UserDbo Create(string email, string username, string password)
    {
        return new UserDbo() { Email = email, Username = username, Password = password };
    }
    
    public static UserDbo Create(string email, string username, string password, string territory)
    {
        return new UserDbo() { Email = email, Username = username, Password = password, TerritoryId = territory};
    }
    
    [Key]
    public Guid UserId { get; }
        
    [Required]
    [EmailAddress]
    public string Email { get; private set; }

    [Required]
    [MinLength(3)]
    [StringLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
    public string Username { get; private set; }

    [Required]
    [MinLength(6)]
    public string Password { get; private set; }

    public TerritoryDbo Territory { get; set; }
    public string TerritoryId { get; set; }

    public DateTime? SuspendTs { get; private set; }

    public virtual IEnumerable<SessionDbo> Sessions { get; }
    public virtual IEnumerable<ClaimDbo> Claims { get; }

    public void UpdatePassword(string newPassword)
    {
        Password = newPassword;
    }
    
    private UserDbo()
    {
    }
}