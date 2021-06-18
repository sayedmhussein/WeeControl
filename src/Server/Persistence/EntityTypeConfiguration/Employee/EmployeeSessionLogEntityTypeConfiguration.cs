using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Server.Domain.EntityDbo.EmployeeSchema;

namespace WeeControl.Server.Persistence.EntityTypeConfiguration.Employee
{
    public class EmployeeSessionLogEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeSessionLogDbo>
    {
        public void Configure(EntityTypeBuilder<EmployeeSessionLogDbo> builder)
        {
            builder.ToTable("EmployeeSessionLog".ToSnakeCase(), nameof(Employee).ToSnakeCase());
            builder.HasIndex(x => x.SessionId ).IsUnique(false);
            builder.Property(p => p.LogTs).HasDefaultValue(DateTime.UtcNow);

            if (MySystemDbContext.DbFacade.IsNpgsql())
            {
                builder.Property(p => p.Id).HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                builder.Property(p => p.Id).ValueGeneratedOnAdd();
            }
        }
    }
}
