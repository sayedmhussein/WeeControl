using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.SharedKernel.Contexts.Essentials;

namespace WeeControl.Core.Domain.Contexts.Essentials;

[Table("Customer", Schema = nameof(Essentials))]
public class CustomerDbo : CustomerModel
{
    public static CustomerDbo Create(Guid userId, CustomerModel model)
    {
        return new CustomerDbo()
        {
            UserId = userId, CountryCode = model.CountryCode
        };
    }
    
    [Key]
    public Guid CustomerId { get; }
    
    public Guid UserId { get; set; }
    public UserDbo User { get; set; }

    public IEnumerable<PersonDbo> Persons { get; set; }

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
        builder.Property(x => x.CustomerId).ValueGeneratedOnAdd();

        builder.HasMany<PersonDbo>(x => x.Persons).WithMany();
    }
}