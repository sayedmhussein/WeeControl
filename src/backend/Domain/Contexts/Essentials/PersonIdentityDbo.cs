using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Core.Domain.Contexts.Essentials;

[Table("PersonIdentity", Schema = nameof(Essentials))]
public class PersonIdentityDbo
{
    [Key] public Guid IdentityId { get; set; }

    public Guid PersonId { get; set; }
    public PersonDbo Person { get; }

    [Required] [StringLength(25)] public string Type { get; set; } = string.Empty;

    [Required] [StringLength(25)] public string Number { get; set; } = string.Empty;

    public DateTime IssueDate { get; set; } = DateTime.UtcNow;

    public DateTime? ExpireDate { get; set; }

    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string CountryId { get; set; } = string.Empty;
}

public class PersonIdentityEntityTypeConfig : IEntityTypeConfiguration<PersonIdentityDbo>
{
    public void Configure(EntityTypeBuilder<PersonIdentityDbo> builder)
    {
        builder.Property(x => x.IdentityId).ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.Person)
            .WithMany(x => x.Identities)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}