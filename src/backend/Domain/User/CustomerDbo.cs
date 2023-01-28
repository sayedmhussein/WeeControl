using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeControl.Core.Domain.User;

[Table(nameof(CustomerDbo), Schema = nameof(User))]
public class CustomerDbo
{
    [Key]
    public Guid CustomerId { get; }
    public Guid UserId { get; set; }
    public UserDbo User { get; set; }

    [StringLength(3, MinimumLength = 3)]
    public string CountryCode { get; set; } = string.Empty;

    private CustomerDbo()
    {
    }

    public CustomerDbo(Guid userId, string country)
    {
        UserId = userId;
        CountryCode = country.Trim();
    }
}

public class CustomerEntityTypeConfig : IEntityTypeConfiguration<CustomerDbo>
{
    public void Configure(EntityTypeBuilder<CustomerDbo> builder)
    {
        builder.Property(x => x.CustomerId).HasDefaultValue(Guid.NewGuid());
    }
}