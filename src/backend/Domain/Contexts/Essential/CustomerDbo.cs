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
    public Guid CustomerId { get; set; }

    public Guid UserId { get; set; }
    public UserDbo User { get; set; }
}

public class CustomerEntityTypeConfig : IEntityTypeConfiguration<CustomerDbo>
{
    public void Configure(EntityTypeBuilder<CustomerDbo> builder)
    {
    }
}