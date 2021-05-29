using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySystem.Domain.EntityDbo;

namespace MySystem.Persistence.EntityTypeConfiguration
{
    public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeDbo>
    {
        public void Configure(EntityTypeBuilder<EmployeeDbo> builder)
        {
            builder.Property(p => p.IsProductive).HasDefaultValue(false);

            builder.ToTable("Employee", "People");
            builder.HasComment("This table inherts from Person table.");
            builder.HasIndex(x => x.Username).IsUnique(true);
            builder.HasIndex(x => x.OfficeId).IsUnique(false);

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
