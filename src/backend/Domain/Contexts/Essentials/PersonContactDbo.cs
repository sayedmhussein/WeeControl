using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.DomainModel.Essentials;

namespace WeeControl.Core.Domain.Contexts.Essentials;

[Table("PersonContact", Schema = nameof(Essentials))]
public class PersonContactDbo : ContactModel
{
    private PersonContactDbo()
    {
    }

    [Key] public Guid ContactId { get; private set; }

    public Guid PersonId { get; set; }
    public PersonDbo Person { get; set; }

    public static PersonContactDbo Create(Guid personId, ContactTypeEnum type, string value)
    {
        var dbo = new PersonContactDbo
        {
            PersonId = personId, ContactValue = value.Trim().ToUpper(),
            ContactType = Enum.GetName(typeof(ContactTypeEnum), type) ?? string.Empty
        };

        return dbo;
    }
}

public class PersonContactEntityTypeConfig : IEntityTypeConfiguration<PersonContactDbo>
{
    public void Configure(EntityTypeBuilder<PersonContactDbo> builder)
    {
        builder.Property(x => x.ContactId).ValueGeneratedOnAdd();

        builder
            .HasOne(x => x.Person)
            .WithMany(x => x.Contacts)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}