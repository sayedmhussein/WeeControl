using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WeeControl.SharedKernel.Essential.Interfaces;

namespace WeeControl.Domain.Contexts.Essential;

public class UserDbo : IUserModel
{
    public static UserDbo Create(string firstname, string lastname, string email, string username, string password,
        string mobileNo, string territory, string nationality)
    {
        return new UserDbo()
        {
            FirstName = firstname.Trim(), LastName = lastname.Trim(),
            Email = email.Trim(), Username = username.Trim(), Password = password,
            MobileNo = mobileNo,
            TerritoryId = territory, Nationality = nationality
        };
    }

    public static UserDbo Create(IUserModel model)
    {
        return new UserDbo()
        {
            FirstName = model.FirstName.Trim(), SecondName = model.SecondName.Trim(),
            ThirdName = model.ThirdName.Trim(), LastName = model.LastName.Trim(),
            Email = model.Email.Trim(), Username = model.Username.Trim(), Password = model.Password,
            MobileNo = model.MobileNo.Trim(),
            TerritoryId = model.TerritoryId, Nationality = model.Nationality
        };
    }
    
    public Guid UserId { get; }
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string ThirdName { get; set; }
    public string LastName { get; set; }
    
    [EmailAddress] public string Email { get; set; }
    [MinLength(3)] public string Username { get; set; }
    public string Password { get; set; }
    [Phone] public string MobileNo { get; set; }
    
    public string TerritoryId { get; set; }
    public TerritoryDbo Territory { get; set; }

    public string Nationality { get; set; }

    [AllowNull]
    public string SuspendArgs { get; private set; }
    
    public string TempPassword { get; private set; }
    public DateTime? TempPasswordTs { get; private set; }

    public string PhotoUrl { get; set; }

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