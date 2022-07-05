using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WeeControl.Domain.Contexts.Essential;

public class UserDbo
{
    public static UserDbo Create(string firstname, string lastname, string email, string username, string password,
        string mobileNo, string territory, string nationality)
    {
        return new UserDbo()
        {
            FirstName = firstname, LastName = lastname,
            Email = email, Username = username, Password = password,
            MobileNo = mobileNo,
            TerritoryId = territory, Nationality = "EGP"
        };
    }
    
    public Guid UserId { get; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    [EmailAddress]
    public string Email { get; private set; }
    
    [MinLength(3)]
    public string Username { get; private set; }
    public string Password { get; private set; }

    [Phone]
    public string MobileNo { get; set; }
    
    public string TerritoryId { get; set; }
    public TerritoryDbo Territory { get; set; }

    public string Nationality { get; set; }

    [AllowNull]
    public string SuspendArgs { get; private set; }
    
    [AllowNull]
    public string TempPassword { get; private set; }
    
    public DateTime? TempPasswordTs { get; private set; }

    public virtual IEnumerable<SessionDbo> Sessions { get; }
    public virtual ICollection<ClaimDbo> Claims { get; }
    
    public virtual ICollection<IdentityDbo> Identities { get; }
    
    public virtual IEnumerable<NotificationDbo> Notifications { get; }

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