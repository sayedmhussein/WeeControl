using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySystem.Shared.Library.Dbo;
using MySystem.Shared.Library.Dbo.Entity;

namespace MySystem.Web.Infrastructure.EfRepository.EntityTypeConfiguration
{
    public class EmployeeClaimEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeClaimDbo>
    {
        public void Configure(EntityTypeBuilder<EmployeeClaimDbo> builder)
        {
            builder.ToTable("EmployeeClaim", "People");
            builder.HasIndex(x => x.EmployeeId).IsUnique(false);
            builder.Property(p => p.GrantedTs).HasDefaultValue(DateTime.UtcNow);

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
