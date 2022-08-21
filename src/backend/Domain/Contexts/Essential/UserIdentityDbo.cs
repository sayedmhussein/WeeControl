using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.SharedKernel.Contexts.Essential.Entities;

namespace WeeControl.ApiApp.Domain.Contexts.Essential;

[Table(nameof(UserIdentityDbo), Schema = nameof(Essential))]
public class UserIdentityDbo : UserIdentityEntity
{
    [Key]
    public Guid IdentityId { get; set; }
    public Guid UserId { get; set; }
    
}

public class UserIdentityEntityTypeConfig : IEntityTypeConfiguration<UserIdentityDbo>
{
    public void Configure(EntityTypeBuilder<UserIdentityDbo> builder)
    {
        builder.Property(x => x.IdentityId).ValueGeneratedOnAdd();
    }
}