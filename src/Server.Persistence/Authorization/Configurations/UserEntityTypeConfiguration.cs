using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Server.Domain.Authorization.Entities;

namespace WeeControl.Server.Persistence.Authorization.Configurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User), nameof(Authorization));
            
            builder.Property(p => p.UserId).ValueGeneratedOnAdd();

            builder.HasMany(x => x.Claims).WithOne(x => x.User);
        }
    }
}