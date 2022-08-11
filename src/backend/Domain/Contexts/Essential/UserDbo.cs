using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.SharedKernel.Essential.Entities;

namespace WeeControl.Domain.Contexts.Essential;

[Table(nameof(UserDbo), Schema = nameof(Essential))]
public class UserDbo : UserEntity
{
    [Key]
    public Guid UserId { get; }

    public PersonDbo Person { get; set; }

    [AllowNull]
    public string SuspendArgs { get; private set; }
    
    public string TempPassword { get; private set; }
    public DateTime? TempPasswordTs { get; private set; }

    public string PhotoUrl { get; set; }

    public virtual IEnumerable<UserSessionDbo> Sessions { get; }
    public virtual ICollection<UserClaimDbo> Claims { get; }
    
    public virtual ICollection<UserIdentityDbo> Identities { get; }
    
    public virtual IEnumerable<UserNotificationDbo> Notifications { get; }

    private UserDbo()
    {
    }

    public UserDbo(UserEntity user)
    {
        Email = user.Email.Trim();
        Username = user.Username.Trim();
        Password = user.Password.Trim();
        MobileNo = user.MobileNo.Trim();
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
        builder.HasComment("Table which holds list of users with their credentials.");
        
        builder.HasIndex(x => x.Email).IsUnique();
        
        builder.HasIndex(x => x.Username).IsUnique();
        
        builder.HasIndex(x => new {x.Username, x.Password}).IsUnique(false);
        builder.Property(p => p.TempPassword).HasMaxLength(128);

        builder.Property(p => p.SuspendArgs).HasMaxLength(255);

        builder.Property(p => p.PhotoUrl).HasMaxLength(255);

        builder.HasOne(x=> x.Person)
            .WithOne(x => x.User)
            .HasForeignKey<PersonDbo>(x => x.UserId);
        
        builder.HasOne<EmployeeDbo>()
            .WithOne(x => x.User)
            .HasForeignKey<EmployeeDbo>(x => x.UserId);
        
        builder.HasOne<CustomerDbo>()
            .WithOne(x => x.User)
            .HasForeignKey<CustomerDbo>(x => x.UserId);

        builder.HasMany(x => x.Claims)
            .WithOne().HasForeignKey(x => x.UserId);
            
        builder.HasMany(x => x.Notifications)
            .WithOne().HasForeignKey(x => x.UserId);
            
        builder.HasMany(x => x.Identities)
            .WithOne().HasForeignKey(x => x.UserId);
    }
}