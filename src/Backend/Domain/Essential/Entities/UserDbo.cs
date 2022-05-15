using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeeControl.Domain.Essential.Entities;

public class UserDbo
{
    [Key]
    public Guid UserId { get; set; }
        
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(3)]
    [StringLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
    public string Username { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    public string TerritoryCode { get; set; }

    public DateTime? SuspendTs { get; set; }

    public virtual IEnumerable<SessionDbo> Sessions { get; set; }
    public virtual IEnumerable<ClaimDbo> Claims { get; set; }

    public static UserDbo Create(string email, string username, string password)
    {
        return new UserDbo() { Email = email, Username = username, Password = password };
    }
        
    public static UserDbo Create(string email, string username, string password, string territory)
    {
        return new UserDbo() { Email = email, Username = username, Password = password, TerritoryCode = territory};
    }

    private UserDbo()
    {
    }
}