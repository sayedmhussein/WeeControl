using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySystem.Domain.EntityDbo;
using MySystem.Domain.EntityDbo.EmployeeSchema;

namespace MySystem.Persistence.EntityTypeConfiguration.Employee
{
    public class EmployeeSessionClaimEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeSessionDbo>
    {
        public void Configure(EntityTypeBuilder<EmployeeSessionDbo> builder)
        {
            builder.ToTable("EmployeeSession", nameof(Employee));
            builder.HasIndex(x => new { x.EmployeeId, x.DeviceId, x.TerminationTs }).IsUnique(false);
            builder.Property(p => p.CreationTs).HasDefaultValue(DateTime.UtcNow);

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
