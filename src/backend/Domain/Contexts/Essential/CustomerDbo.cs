using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.SharedKernel.Essential.Entities;

namespace WeeControl.Domain.Contexts.Essential;

[Table(nameof(CustomerDbo), Schema = nameof(Essential))]
public class CustomerDbo : CustomerEntity
{
    [Key]
    public Guid CustomerId { get; }
    public Guid UserId { get; set; }
    public UserDbo User { get; set; }

    private CustomerDbo()
    {
    }

    public CustomerDbo(CustomerEntity customer)
    {
        CountryCode = customer.CountryCode.Trim();
    }
}

public class CustomerEntityTypeConfig : IEntityTypeConfiguration<CustomerDbo>
{
    public void Configure(EntityTypeBuilder<CustomerDbo> builder)
    {
        builder.Property(x => x.CustomerId).HasDefaultValue(Guid.NewGuid());
    }
}