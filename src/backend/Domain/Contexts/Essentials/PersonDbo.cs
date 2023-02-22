using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.Domain.Exceptions;
using WeeControl.Core.SharedKernel.Contexts.Essentials;

namespace WeeControl.Core.Domain.Contexts.Essentials;

[Table("Person", Schema = nameof(Essentials))]
public class PersonDbo : PersonModel
{
    public static PersonDbo Create(PersonModel model)
    {
        DomainValidationException.ValidateEntity(model);
        
        return new PersonDbo
        {
            FirstName = model.FirstName,
            SecondName = model.SecondName,
            ThirdName = model.ThirdName,
            LastName = model.LastName,
            NationalityCode = model.NationalityCode.ToUpper(),
            DateOfBirth = model.DateOfBirth
        };
    }

    public static PersonDbo Create(string firstName, string lastName, string nationalityCode, DateOnly dob)
    {
        return Create(new PersonModel { FirstName = firstName, LastName = lastName, NationalityCode = nationalityCode, DateOfBirth = dob});
    }

    [Key]
    public Guid PersonId { get; init; }

    public virtual IEnumerable<PersonIdentityDbo> Identities { get; }
    public virtual IEnumerable<AddressDbo> Addresses { get; }
    public virtual IEnumerable<PersonContactDbo> Contacts { get; }
    
    private PersonDbo()
    {
    }
}

public class PersonEntityTypeConfig : IEntityTypeConfiguration<PersonDbo>
{
    public void Configure(EntityTypeBuilder<PersonDbo> builder)
    {
        builder.Property(x => x.PersonId).HasDefaultValue(Guid.NewGuid());
        //builder.Property(x => x.PersonId).ValueGeneratedOnAdd();
        
        builder.HasMany(x => x.Identities)
            .WithOne().HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.Addresses)
            .WithOne().HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Contacts)
            .WithOne().HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}