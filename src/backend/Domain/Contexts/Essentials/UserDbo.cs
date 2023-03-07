using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.Domain.Exceptions;
using WeeControl.Core.SharedKernel;
using WeeControl.Core.SharedKernel.Contexts.Essentials;

namespace WeeControl.Core.Domain.Contexts.Essentials;

[Table(nameof(UserDbo), Schema = nameof(Essentials))]
public class UserDbo : UserModel
{
    public static UserDbo Create(Guid personId, string email, string username, string password)
    {
        return Create(personId, new UserModel(){ Email = email, Username = username, Password = password});
    }

    public static UserDbo Create(Guid personId, UserModel model)
    {
        if (personId == Guid.Empty)
            throw new DomainOutOfRangeException("Person ID can't be empty GUID");
        
        model.ThrowExceptionIfEntityModelNotValid();

        return new UserDbo()
        {
            PersonId = personId, 
            Email = model.Email.Trim().ToLower(), 
            Username = model.Username.Trim().ToLower(),
            Password = model.Password
        };
    }
    
    [Key]
    public Guid UserId { get; }

    public Guid PersonId { get; private set; }
    public PersonDbo Person { get; set; }
    
    
    [AllowNull]
    [StringLength(255)]
    public string SuspendArgs { get; private set; }

    public string TempPassword { get; private set; }
    public DateTime? TempPasswordTs { get; private set; }

    
    [AllowNull]
    [StringLength(255)]
    public string PhotoUrl { get; set; }

    public virtual IEnumerable<UserSessionDbo> Sessions { get; }
    public virtual ICollection<UserClaimDbo> Claims { get; }
    public virtual IEnumerable<UserNotificationDbo> Notifications { get; }

    private UserDbo()
    {
    }

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
        var claim = UserClaimDbo.Create(UserId, claimType, claimValue, grantedBy);
        Claims.Add(claim);
    }
}

public class UserEntityTypeConfig : IEntityTypeConfiguration<UserDbo>
{
    public void Configure(EntityTypeBuilder<UserDbo> builder)
    {
        builder.Property(p => p.UserId).ValueGeneratedOnAdd();

        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasIndex(x => x.Username).IsUnique();

        builder.HasIndex(x => new { x.Username, x.Password }).IsUnique(false);
        builder.Property(p => p.TempPassword).HasMaxLength(128);

        builder.Property(p => p.SuspendArgs).HasMaxLength(255);

        builder.HasOne(x => x.Person)
            .WithOne()
            .HasForeignKey<UserDbo>(x => x.PersonId)
            .IsRequired();

        builder.HasMany(x => x.Claims)
            .WithOne().HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.Notifications)
            .WithOne().HasForeignKey(x => x.UserId);

        
    }
}