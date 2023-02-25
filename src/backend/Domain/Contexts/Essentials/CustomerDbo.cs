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

    private CustomerDbo()
    {
    }
}

public class CustomerEntityTypeConfig : IEntityTypeConfiguration<CustomerDbo>
{
    public void Configure(EntityTypeBuilder<CustomerDbo> builder)
    {
        builder.Property(x => x.CustomerId).ValueGeneratedOnAdd();

        builder.HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<CustomerDbo>(x => x.UserId)
            .IsRequired();
           
    }
}