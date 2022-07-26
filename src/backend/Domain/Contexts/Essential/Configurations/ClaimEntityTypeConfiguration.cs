using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Domain.Contexts.Essential.Configurations
{
    public class ClaimEntityTypeConfiguration : IEntityTypeConfiguration<ClaimDbo>
    {
        public void Configure(EntityTypeBuilder<ClaimDbo> builder)
        {
            builder.ToTable(nameof(ClaimDbo), nameof(Essential));
            builder.HasKey(x => x.ClaimId);
            builder.Property(p => p.ClaimId).ValueGeneratedOnAdd();
            
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
}
