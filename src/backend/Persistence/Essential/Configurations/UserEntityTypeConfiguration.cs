using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Domain.Contexts.Essential;

namespace WeeControl.Persistence.Essential.Configurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserDbo>
    {
        public void Configure(EntityTypeBuilder<UserDbo> builder)
        {
            builder.ToTable(nameof(UserDbo), schema: nameof(Essential));
            builder.HasKey(p => p.UserId);
            builder.HasComment("Table which holds list of users with their credentials.");
            
            builder.Property(p => p.UserId).ValueGeneratedOnAdd();

            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(50);
            
            builder.Property(p => p.Email).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Username).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Password).IsRequired().HasMaxLength(128);
            builder.Property(p => p.TempPassword).HasMaxLength(128);

            builder.Property(p => p.SuspendArgs).HasMaxLength(255);

            builder.Property(p => p.Nationality).HasMaxLength(3);

            builder.HasIndex(x => x.Username).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => new {x.Username, x.Password}).IsUnique(false);
            
            builder.HasMany(x => x.Claims)
                .WithOne().HasForeignKey(x => x.UserId);
        }
    }
}
