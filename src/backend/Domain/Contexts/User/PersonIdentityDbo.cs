using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Core.Domain.Contexts.User;

[Table(nameof(PersonIdentityDbo), Schema = nameof(User))]
public class PersonIdentityDbo
{
    [Key]
    public Guid IdentityId { get; set; }
    
    public Guid PersonId { get; set; }
    public PersonIdentityDbo Person { get; }

    [Required]
    [StringLength(25)]
    public string Type { get; set; } = string.Empty;

    [Required]
    [StringLength(25)]
    public string Number { get; set; } = string.Empty;

    public DateTime IssueDate { get; set; } = DateTime.UtcNow;

    public DateTime? ExpireDate { get; set; } = null;

    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string CountryId { get; set; } = string.Empty;

}

public class UserIdentityEntityTypeConfig : IEntityTypeConfiguration<PersonIdentityDbo>
{
    public void Configure(EntityTypeBuilder<PersonIdentityDbo> builder)
    {
        builder.Property(x => x.IdentityId).ValueGeneratedOnAdd();
    }
}