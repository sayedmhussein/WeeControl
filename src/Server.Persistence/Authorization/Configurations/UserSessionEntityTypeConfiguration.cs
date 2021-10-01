using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Server.Domain.Authorization.Entities;

namespace WeeControl.Server.Persistence.Authorization.Configurations
{
    public class UserSessionEntityTypeConfiguration : IEntityTypeConfiguration<UserSession>
    {
        public void Configure(EntityTypeBuilder<UserSession> builder)
        {
            builder.ToTable(nameof(UserSession), nameof(Authorization));

            builder.Property(p => p.SessionId).ValueGeneratedOnAdd();
            builder.Property(p => p.CreationTs).HasDefaultValue(DateTime.UtcNow);

            builder.OwnsMany(x => x.Logs, xx =>
            {
                xx.WithOwner().HasForeignKey();
                xx.Property(o => o.LogTs).HasDefaultValue(DateTime.UtcNow);
            });

        }
    }
}