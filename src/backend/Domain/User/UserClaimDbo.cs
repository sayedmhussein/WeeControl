using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeeControl.Core.Domain.User;

namespace WeeControl.Core.Domain.User;

[Table(nameof(UserClaimDbo), Schema = nameof(User))]
public class UserClaimDbo
{
    public static UserClaimDbo Create(Guid userId, string type, string tag, Guid grantedById)
    {
        return new UserClaimDbo()
        {
            UserId = userId,
            ClaimType = type,
            ClaimValue = tag,
            GrantedById = grantedById
        };
    }

    [Key]
    public Guid ClaimId { get; }

    public Guid? UserId { get; private set; }
    public UserDbo User { get; }

    [Required]
    [StringLength(5)]
    public string ClaimType { get; private set; }

    public string ClaimValue { get; private set; }

    public DateTime GrantedTs { get; } = DateTime.UtcNow;

    public Guid GrantedById { get; private set; }
    public EmployeeDbo GrantedBy { get; private set; }

    public DateTime? RevokedTs { get; private set; }

    public Guid? RevokedById { get; private set; }
    public EmployeeDbo RevokedBy { get; private set; }

    public void Revoke(Guid revokedById)
    {
        RevokedById = revokedById;
        RevokedTs = DateTime.UtcNow;
    }

    private UserClaimDbo()
    {
    }
}

public class UserClaimEntityTypeConfig : IEntityTypeConfiguration<UserClaimDbo>
{
    public void Configure(EntityTypeBuilder<UserClaimDbo> builder)
    {
        builder.Property(p => p.ClaimId).ValueGeneratedOnAdd().HasDefaultValue(Guid.NewGuid());

        builder.Property(p => p.GrantedTs).HasDefaultValue(DateTime.UtcNow);
        builder.HasIndex(i => new { i.ClaimType, i.ClaimValue }).IsUnique();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.GrantedBy)
            .WithMany()
            .HasForeignKey(x => x.GrantedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.RevokedBy)
            .WithMany()
            .HasForeignKey(x => x.RevokedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}