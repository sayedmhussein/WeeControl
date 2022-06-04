using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WeeControl.Domain.Essential.Entities;

public class UserDbo
{
    [Obsolete("Use the other")]
    public static UserDbo Create(string email, string username, string password)
    {
        return new UserDbo() { Email = email.ToLower(), Username = username.ToLower(), Password = password };
    }
    
    [Obsolete("Use the other")]
    public static UserDbo Create(string email, string username, string password, string territory)
    {
        return new UserDbo() { Email = email.ToLower(), Username = username.ToLower(), Password = password, TerritoryId = territory.ToUpper()};
    }

    public static UserDbo Create(string firstname, string lastname, string email, string username, string password,
        string mobileNo, string territory)
    {
        return new UserDbo()
        {
            FirstName = firstname, LastName = lastname,
            Email = email, Username = username, Password = password,
            MobileNo = mobileNo,
            TerritoryId = territory
        };
    }
    
    [Key]
    public Guid UserId { get; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }
    
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

    [Phone]
    public string MobileNo { get; set; }
    
    public string TerritoryId { get; set; }
    public TerritoryDbo Territory { get; set; }

    public string Nationality { get; set; }

    [AllowNull]
    [StringLength(255)]
    public string SuspendArgs { get; private set; }
    
    [AllowNull]
    [StringLength(128)]
    public string TempPassword { get; private set; }
    
    public DateTime? TempPasswordTs { get; private set; }

    public virtual IEnumerable<SessionDbo> Sessions { get; }
    public virtual ICollection<ClaimDbo> Claims { get; }
    
    public virtual ICollection<IdentityDbo> Identities { get; }

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