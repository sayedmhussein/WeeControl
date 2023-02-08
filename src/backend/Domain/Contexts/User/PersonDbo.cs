using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.SharedKernel.Contexts.User;

namespace WeeControl.Core.Domain.Contexts.User;

[Table("Person", Schema = nameof(User))]
public class PersonDbo : PersonModel
{
    public static PersonDbo Create(PersonModel model)
    {
        return new PersonDbo
        {
            FirstName = model.FirstName,
            SecondName = model.SecondName,
            ThirdName = model.ThirdName,
            LastName = model.LastName,
            Nationality = model.Nationality,
            DateOfBirth = model.DateOfBirth
        };
    }

    public static PersonDbo Create(string firstName, string lastName, string nationality, DateOnly dob)
    {
        return Create(new PersonModel { FirstName = firstName, LastName = lastName, Nationality = nationality, DateOfBirth = dob});
    }

    [Key]
    public Guid PersonId { get; }

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
        builder.Property(x => x.PersonId).ValueGeneratedOnAdd();//.HasDefaultValue(Guid.NewGuid());
        
        builder.HasMany(x => x.Identities)
            .WithOne().HasForeignKey(x => x.PersonId);
        
        builder.HasMany(x => x.Addresses)
            .WithOne().HasForeignKey(x => x.PersonId);
        
        builder.HasMany(x => x.Contacts)
            .WithOne().HasForeignKey(x => x.PersonId);
    }
}