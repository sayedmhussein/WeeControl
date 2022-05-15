using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Backend.Domain.Essential.Entities;

namespace WeeControl.Backend.Persistence.Essential.Configurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserDbo>
    {
        public void Configure(EntityTypeBuilder<UserDbo> builder)
        {
            builder.ToTable(nameof(UserDbo), nameof(Essential));
            builder.HasKey(p => p.UserId);
            builder.Property(p => p.UserId).ValueGeneratedOnAdd();

            builder.HasMany(x => x.Claims).WithOne().HasForeignKey(x => x.UserId);
        }
    }
}
