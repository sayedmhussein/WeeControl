using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Server.Domain.BasicDbos.EmployeeSchema;

namespace WeeControl.Server.Persistence.EntityTypeConfiguration.Employee
{
    public class EmployeeIdentityEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeIdentityDbo>
    {
        public void Configure(EntityTypeBuilder<EmployeeIdentityDbo> builder)
        {
            builder.ToTable("EmployeeIdentity".ToSnakeCase(), nameof(Employee).ToSnakeCase());
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
