using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySystem.Domain.EntityDbo.EmployeeSchema;

namespace MySystem.Persistence.EntityTypeConfiguration.EmployeeSchema
{
    public class EmployeeIdentityEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeIdentityDbo>
    {
        public void Configure(EntityTypeBuilder<EmployeeIdentityDbo> builder)
        {
            builder.HasIndex(x => x.EmployeeId).IsUnique(false);

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
