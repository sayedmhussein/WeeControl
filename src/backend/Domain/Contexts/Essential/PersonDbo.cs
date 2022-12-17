using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Common.SharedKernel.Contexts.Temporary.Entities;

namespace WeeControl.ApiApp.Domain.Contexts.Essential;

[Table(nameof(PersonDbo), Schema = nameof(Essential))]
public class PersonDbo : PersonalEntity
{
    [Key]
    public Guid PersonId { get; }

    public Guid UserId { get; }
    public UserDbo User { get; }

    private PersonDbo()
    {
    }

    public PersonDbo(Guid userId, PersonalEntity person)
    {
        UserId = userId;
        FirstName = person.FirstName.Trim();
        SecondName = person.SecondName?.Trim();
        ThirdName = person.ThirdName?.Trim();
        LastName = person.LastName.Trim();
        Nationality = person.Nationality.Trim().ToUpper();
    }
}

public class PersonEntityTypeConfig : IEntityTypeConfiguration<PersonDbo>
{
    public void Configure(EntityTypeBuilder<PersonDbo> builder)
    {
        builder.Property(x => x.PersonId).ValueGeneratedOnAdd().HasDefaultValue(Guid.NewGuid());
    }
}