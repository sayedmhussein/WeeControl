using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Backend.Domain.Credentials.DatabaseObjects;

namespace WeeControl.Backend.Persistence.Credentials.Configurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserDbo>
    {
        public void Configure(EntityTypeBuilder<UserDbo> builder)
        {
            builder.ToTable("credentials", "user");
            builder.Property(p => p.UserId).ValueGeneratedOnAdd();
        }
    }
}
