using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Core.Domain.Contexts.Essentials;

[Table("UserClaim", Schema = nameof(Essentials))]
public class UserClaimDbo
{
    private UserClaimDbo()
    {
    }

    [Key] public Guid ClaimId { get; }

    [Required] public Guid? UserId { get; private set; }
    public UserDbo User { get; }

    [Required] [StringLength(5)] public string ClaimType { get; private set; }

    public string ClaimValue { get; private set; }

    public DateTime GrantedTs { get; } = DateTime.UtcNow;

    [Required] public Guid GrantedById { get; private set; }

    public UserDbo GrantedBy { get; private set; }

    public DateTime? RevokedTs { get; private set; }
    public Guid? RevokedById { get; private set; }
    public UserDbo RevokedBy { get; private set; }

    public static UserClaimDbo Create(Guid userId, string type, string tag, Guid grantedById)
    {
        return new UserClaimDbo
        {
            UserId = userId,
            ClaimType = type,
            ClaimValue = tag,
            GrantedById = grantedById
        };
    }

    public void Revoke(Guid revokedById)
    {
        RevokedById = revokedById;
        RevokedTs = DateTime.UtcNow;
    }
}

public class UserClaimEntityTypeConfig : IEntityTypeConfiguration<UserClaimDbo>
{
    public void Configure(EntityTypeBuilder<UserClaimDbo> builder)
    {
        builder.Property(p => p.ClaimId).ValueGeneratedOnAdd(); //.HasDefaultValue(Guid.NewGuid());

        builder.Property(p => p.GrantedTs).ValueGeneratedOnAdd();

        builder.HasIndex(i => new {i.UserId, i.ClaimType, i.ClaimValue}).IsUnique();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Claims)
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.GrantedBy)
            .WithMany()
            .HasForeignKey(x => x.GrantedById)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.RevokedBy)
            .WithMany()
            .HasForeignKey(x => x.RevokedById)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}