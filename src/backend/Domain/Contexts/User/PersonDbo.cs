using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.SharedKernel.Contexts.User;

namespace WeeControl.Core.Domain.Contexts.User;

[Table(nameof(PersonDbo), Schema = nameof(User))]
public class PersonDbo : PersonModel
{
    [Obsolete]
    public static PersonDbo Create(Guid userId, string firstName, string lastName, string nationality,
        DateOnly dateOfBirth)
    {
        return new PersonDbo()
        {
            UserId = userId,
            FirstName = firstName, LastName = lastName, Nationality = nationality, DateOfBirth = dateOfBirth
        };
    }

    public static PersonDbo Create(PersonModel model)
    {
        var dbo = new PersonDbo();
        //To check each of the below and raise exception if not correct!
        dbo.FirstName = model.FirstName;
        dbo.LastName = model.LastName;
        dbo.Nationality = model.Nationality;
        dbo.DateOfBirth = model.DateOfBirth;
        return dbo;
    }

[Key]
    public Guid PersonId { get; }

    public Guid UserId { get; private set; }
    public UserDbo User { get; }
    
    private PersonDbo()
    {
    }
}

public class PersonEntityTypeConfig : IEntityTypeConfiguration<PersonDbo>
{
    public void Configure(EntityTypeBuilder<PersonDbo> builder)
    {
        builder.Property(x => x.PersonId).ValueGeneratedOnAdd().HasDefaultValue(Guid.NewGuid());
    }
}