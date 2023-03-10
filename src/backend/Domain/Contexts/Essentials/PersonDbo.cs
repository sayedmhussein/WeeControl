using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Core.SharedKernel.Contexts.Essentials;
using WeeControl.Core.SharedKernel.ExtensionMethods;

namespace WeeControl.Core.Domain.Contexts.Essentials;

[Table("Person", Schema = nameof(Essentials))]
public class PersonDbo : PersonModel
{
    private PersonDbo()
    {
    }

    [Key] public Guid PersonId { get; init; }

    public virtual IEnumerable<PersonIdentityDbo> Identities { get; }
    public virtual IEnumerable<AddressDbo> Addresses { get; }
    public virtual IEnumerable<PersonContactDbo> Contacts { get; }

    public static PersonDbo Create(PersonModel model)
    {
        model.ThrowExceptionIfEntityModelNotValid();

        return new PersonDbo
        {
            FirstName = model.FirstName.ToUpperFirstLetter(),
            SecondName = model.SecondName?.ToUpperFirstLetter(),
            ThirdName = model.ThirdName?.ToUpperFirstLetter(),
            LastName = model.LastName.ToUpperFirstLetter(),
            NationalityCode = model.NationalityCode.ToUpper(),
            DateOfBirth = model.DateOfBirth
        };
    }
}

public class PersonEntityTypeConfig : IEntityTypeConfiguration<PersonDbo>
{
    public void Configure(EntityTypeBuilder<PersonDbo> builder)
    {
        builder.Property(x => x.PersonId).ValueGeneratedOnAdd();
    }
}