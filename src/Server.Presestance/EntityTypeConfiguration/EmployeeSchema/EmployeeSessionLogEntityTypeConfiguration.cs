using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySystem.Domain.EntityDbo;
using MySystem.Domain.EntityDbo.EmployeeSchema;

namespace MySystem.Persistence.EntityTypeConfiguration.EmployeeSchema
{
    public class EmployeeSessionLogEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeSessionLogDbo>
    {
        public void Configure(EntityTypeBuilder<EmployeeSessionLogDbo> builder)
        {
            builder.ToTable("EmployeeSessionLog", "People");
            builder.HasIndex(x => x.SessionId ).IsUnique(false);
            builder.Property(p => p.LogTs).HasDefaultValue(DateTime.UtcNow);

            if (MySystemDbContext.DbFacade.IsNpgsql())
            {
                builder
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                builder.Property(p => p.Id).ValueGeneratedOnAdd();
            }
        }
    }
}
