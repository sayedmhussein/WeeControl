using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Core.Domain.Contexts.User;

[Table(nameof(PersonDbo), Schema = nameof(User))]
public class PersonDbo
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

    [Required]
    [StringLength(45, MinimumLength = 1)]
    public string FirstName { get; set; } = string.Empty;

    [AllowNull]
    [StringLength(45)]
    public string SecondName { get; set; } = string.Empty;

    [AllowNull]
    [StringLength(45)]
    public string ThirdName { get; set; } = string.Empty;

    [Required]
    [StringLength(45, MinimumLength = 1)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(3, MinimumLength = 3)]
    public string Nationality { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; }

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