using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Backend.Domain.EntityGroup.EmployeeSchema;

namespace WeeControl.Backend.Persistence.EntityTypeConfiguration.Employee
{
    public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeDbo>
    {
        public void Configure(EntityTypeBuilder<EmployeeDbo> builder)
        {
            builder.ToTable("Employee".ToSnakeCase(), nameof(Employee).ToSnakeCase());
            builder.HasComment("-");

            if (MySystemDbContext.DbFacade.IsNpgsql())
            {
                builder.Property(p => p.Id).HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                builder.Property(p => p.Id).ValueGeneratedOnAdd();
            }

            builder.Property(p => p.IsProductive).HasDefaultValue(false);

            builder.HasIndex(x => x.Username).IsUnique(true);
            builder.HasIndex(x => x.Email).IsUnique(true);
            builder.HasIndex(x => x.TerritoryId).IsUnique(false);
            builder.HasIndex(x => new { x.Username, x.Password, x.AccountLockArgument }).IsUnique(false);
            builder.HasIndex(x => new { x.Email, x.Password, x.AccountLockArgument }).IsUnique(false);
        }
    }
}
