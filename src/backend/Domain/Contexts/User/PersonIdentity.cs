using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Core.Domain.Contexts.User;

[Table(nameof(PersonIdentity), Schema = nameof(User))]
public class PersonIdentity
{
    [Key]
    public Guid IdentityId { get; set; }
    public Guid UserId { get; set; }

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

public class UserIdentityEntityTypeConfig : IEntityTypeConfiguration<PersonIdentity>
{
    public void Configure(EntityTypeBuilder<PersonIdentity> builder)
    {
        builder.Property(x => x.IdentityId).ValueGeneratedOnAdd();
    }
}