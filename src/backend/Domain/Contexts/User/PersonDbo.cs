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
    public static PersonDbo Create(Guid userId, string firstName, string lastName, string nationality, DateOnly dateOfBirth)
    {
        return new PersonDbo()
        {
            UserId = userId,
            FirstName = firstName, LastName = lastName, Nationality = nationality, DateOfBirth = dateOfBirth
        };
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