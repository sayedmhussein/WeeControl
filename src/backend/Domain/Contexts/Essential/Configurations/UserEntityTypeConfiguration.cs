using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WeeControl.Domain.Contexts.Essential.Configurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserDbo>
    {
        public void Configure(EntityTypeBuilder<UserDbo> builder)
        {
            builder.ToTable(nameof(UserDbo), schema: nameof(Essential));
            builder.HasKey(p => p.UserId);
            builder.Property(p => p.UserId).ValueGeneratedOnAdd();
            builder.HasComment("Table which holds list of users with their credentials.");
            
            builder.Property(p => p.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(p => p.SecondName).HasMaxLength(50);
            builder.Property(p => p.ThirdName).HasMaxLength(50);
            builder.Property(p => p.LastName).HasMaxLength(50).IsRequired();
            
            builder.Property(p => p.Email).HasMaxLength(255).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            
            builder.Property(p => p.Username).HasMaxLength(50).IsRequired();
            builder.HasIndex(x => x.Username).IsUnique();
            
            builder.Property(p => p.Password).IsRequired().HasMaxLength(128);
            builder.HasIndex(x => new {x.Username, x.Password}).IsUnique(false);
            builder.Property(p => p.TempPassword).HasMaxLength(128);

            builder.Property(p => p.SuspendArgs).HasMaxLength(255);
            builder.Property(p => p.Nationality).HasMaxLength(3);

            builder.Property(p => p.PhotoUrl).HasMaxLength(255);

            builder.HasOne(x => x.Territory)
                .WithMany()
                .HasForeignKey(x => x.TerritoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(x => x.Claims)
                .WithOne().HasForeignKey(x => x.UserId);
            
            builder.HasMany(x => x.Notifications)
                .WithOne().HasForeignKey(x => x.UserId);
            
            builder.HasMany(x => x.Identities)
                .WithOne().HasForeignKey(x => x.UserId);
        }
    }
}
