using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WeeControl.Domain.Essential.Entities;

public class UserDbo
{
    public static UserDbo Create(string email, string username, string password)
    {
        return new UserDbo() { Email = email.ToLower(), Username = username.ToLower(), Password = password };
    }
    
    public static UserDbo Create(string email, string username, string password, string territory)
    {
        return new UserDbo() { Email = email.ToLower(), Username = username.ToLower(), Password = password, TerritoryId = territory.ToUpper()};
    }
    
    [Key]
    public Guid UserId { get; }
        
    [Required]
    [EmailAddress]
    [StringLength(50)]
    public string Email { get; private set; }

    [Required]
    [MinLength(3)]
    [StringLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
    public string Username { get; private set; }

    [Required]
    [StringLength(128)]
    public string Password { get; private set; }
    
    public string TerritoryId { get; set; }
    public TerritoryDbo Territory { get; set; }

    [AllowNull]
    [StringLength(255)]
    public string SuspendArgs { get; private set; }
    
    [AllowNull]
    [StringLength(128)]
    public string TempPassword { get; private set; }
    
    public DateTime? TempPasswordTs { get; private set; }

    public virtual IEnumerable<SessionDbo> Sessions { get; }
    public virtual ICollection<ClaimDbo> Claims { get; }

    public void UpdatePassword(string newPassword)
    {
        Password = newPassword;
    }

    public void Suspend(string reason)
    {
        SuspendArgs = DateTime.UtcNow.ToLongDateString() + " - " + reason;
    }

    public void Activate()
    {
        SuspendArgs = null;
    }

    public void SetTemporaryPassword(string password)
    {
        TempPassword = password;
        TempPasswordTs = DateTime.UtcNow;
    }

    public void AddClaim(string claimType, string claimValue, Guid grantedBy)
    {
        var claim = ClaimDbo.Create(UserId, claimType, claimValue, grantedBy);
        Claims.Add(claim);
    }
    
    private UserDbo()
    {
    }
}