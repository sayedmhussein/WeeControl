using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.SharedKernel.Contexts.User;

namespace WeeControl.Core.Domain.Contexts.User;

[Table("Address", Schema = nameof(User))]
public class AddressDbo : AddressModel
{
    [Key] 
    public Guid AddressId { get; set; }

    public Guid PersonId { get; set; }
    public PersonDbo Person { get; set; }
}

public class PersonAddressEntityTypeConfig : IEntityTypeConfiguration<AddressDbo>
{
    public void Configure(EntityTypeBuilder<AddressDbo> builder)
    {
        builder.Property(x => x.AddressId).ValueGeneratedOnAdd();
        builder
            .HasOne<PersonDbo>()
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}