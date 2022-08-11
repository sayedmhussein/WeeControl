using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.SharedKernel.Essential.Entities;

namespace WeeControl.Domain.Contexts.Essential;

[Table(nameof(PersonDbo), Schema = nameof(Essential))]
public class PersonDbo : PersonalEntity
{
    [Key]
    public Guid PersonId { get; }

    public Guid UserId { get; set; }
    public UserDbo User { get; set; }

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
        builder.Property(x => x.PersonId).HasDefaultValue(Guid.NewGuid());
    }
}