using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.SharedKernel.Contexts.User;

namespace WeeControl.Core.Domain.Contexts.User;

[Table("PersonContact", Schema = nameof(User))]
public class PersonContactDbo : ContactModel
{
    [Key]
    public Guid ContactId { get; private set; }
    
    public Guid PersonId { get; set; }
    public PersonDbo Person { get; set; }
}

public class PersonContactEntityTypeConfig : IEntityTypeConfiguration<PersonContactDbo>
{
    public void Configure(EntityTypeBuilder<PersonContactDbo> builder)
    {
        builder.Property(x => x.ContactId).ValueGeneratedOnAdd();
    }
}