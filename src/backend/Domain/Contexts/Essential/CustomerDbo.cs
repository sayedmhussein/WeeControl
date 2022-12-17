using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Common.SharedKernel.Contexts.Temporary.Entities;

namespace WeeControl.ApiApp.Domain.Contexts.Essential;

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

    public CustomerDbo(Guid userId, CustomerEntity customer)
    {
        UserId = userId;
        CountryCode = customer.CountryCode.Trim();
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