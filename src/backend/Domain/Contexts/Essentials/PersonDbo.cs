using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.DomainModel.Essentials;
using WeeControl.Core.SharedKernel.ExtensionHelpers;

namespace WeeControl.Core.Domain.Contexts.Essentials;

[Table("Person", Schema = nameof(Essentials))]
public class PersonDbo : PersonModel
{
    private PersonDbo()
    {
    }

    [Key] public Guid PersonId { get; init; }

    [AllowNull] [StringLength(255)] public string SuspendArgs { get; private set; }

    public string TempPassword { get; private set; }
    public DateTime? TempPasswordTs { get; private set; }


    [AllowNull] [StringLength(255)] public string PhotoUrl { get; set; }

    public virtual IEnumerable<PersonIdentityDbo> Identities { get; }
    public virtual IEnumerable<AddressDbo> Addresses { get; }
    public virtual IEnumerable<PersonContactDbo> Contacts { get; }

    public virtual IEnumerable<UserSessionDbo> Sessions { get; }
    public virtual ICollection<UserClaimDbo> Claims { get; }
    public virtual IEnumerable<UserNotificationDbo> Notifications { get; }

    public static PersonDbo Create(PersonModel model)
    {
        model.ThrowExceptionIfEntityModelNotValid();

        return new PersonDbo
        {
            FirstName = model.FirstName.ToUpperFirstLetter(),
            SecondName = model.SecondName?.ToUpperFirstLetter(),
            ThirdName = model.ThirdName?.ToUpperFirstLetter(),
            LastName = model.LastName.ToUpperFirstLetter(),
            NationalityCode = model.NationalityCode.ToUpper(),
            DateOfBirth = model.DateOfBirth,
            Email = model.Email.Trim().ToLower(),
            Username = model.Username.Trim().ToLower(),
            Password = model.Password
        };
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
        var claim = UserClaimDbo.Create(PersonId, claimType, claimValue, grantedBy);
        Claims.Add(claim);
    }
}

public class PersonEntityTypeConfig : IEntityTypeConfiguration<PersonDbo>
{
    public void Configure(EntityTypeBuilder<PersonDbo> builder)
    {
        builder.Property(x => x.PersonId).ValueGeneratedOnAdd();

        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasIndex(x => x.Username).IsUnique();

        builder.HasIndex(x => new {x.Username, x.Password}).IsUnique(false);
        builder.Property(p => p.TempPassword).HasMaxLength(128);

        builder.Property(p => p.SuspendArgs).HasMaxLength(255);

        builder.HasMany(x => x.Claims)
            .WithOne().HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.Notifications)
            .WithOne().HasForeignKey(x => x.UserId);
    }
}