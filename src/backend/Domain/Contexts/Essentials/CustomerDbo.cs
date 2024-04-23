using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.DomainModel.Essentials;

namespace WeeControl.Core.Domain.Contexts.Essentials;

[Table("Customer", Schema = nameof(Essentials))]
public class CustomerDbo : CustomerModel
{
    private CustomerDbo()
    {
    }

    [Key] public Guid CustomerId { get; }

    public Guid UserId { get; set; }
    public PersonDbo User { get; set; }

    public static CustomerDbo Create(Guid userId, CustomerModel model)
    {
        return new CustomerDbo
        {
            UserId = userId, CountryCode = model.CountryCode
        };
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