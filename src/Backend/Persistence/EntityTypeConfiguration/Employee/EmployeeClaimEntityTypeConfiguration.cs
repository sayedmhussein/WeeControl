using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Backend.Domain.EntityGroups.Employee;

namespace WeeControl.Backend.Persistence.EntityTypeConfiguration.Employee
{
    public class EmployeeClaimEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeClaimDbo>
    {
        public void Configure(EntityTypeBuilder<EmployeeClaimDbo> builder)
        {
            builder.ToTable("EmployeeClaim".ToSnakeCase(), nameof(Employee).ToSnakeCase());
            builder.HasIndex(x => x.EmployeeId).IsUnique(false);
            builder.Property(p => p.GrantedTs).HasDefaultValue(DateTime.UtcNow);
            builder.HasOne(x => x.Employee).WithMany(x => x.Claims).HasForeignKey(x => x.EmployeeId);
            //builder.HasOne(x => x.Employee).WithMany(x => x.Claims).HasForeignKey(x => x.GrantedById);
            //builder.HasOne(x => x.Employee).WithMany(x => x.Claims).HasForeignKey(x => x.RevokedById);

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
