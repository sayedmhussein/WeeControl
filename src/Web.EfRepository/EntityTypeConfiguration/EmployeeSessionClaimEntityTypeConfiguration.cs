using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySystem.Shared.Library.Dbo;
using MySystem.Shared.Library.Dbo.Entity;

namespace MySystem.Web.Infrastructure.EfRepository.EntityTypeConfiguration
{
    public class EmployeeSessionClaimEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeSessionDbo>
    {
        public void Configure(EntityTypeBuilder<EmployeeSessionDbo> builder)
        {
            builder.ToTable("EmployeeSession", "People");
            builder.HasIndex(x => new { x.EmployeeId, x.DeviceId, x.TerminationTs }).IsUnique(false);
            builder.Property(p => p.CreationTs).HasDefaultValue(DateTime.UtcNow);

            if (DataContext.DbFacade.IsNpgsql())
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
