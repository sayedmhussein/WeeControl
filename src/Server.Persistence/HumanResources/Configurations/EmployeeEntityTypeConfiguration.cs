using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Server.Domain.HumanResources.Entities;

namespace WeeControl.Server.Persistence.HumanResources.Configurations
{
    public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable(nameof(Employee), nameof(HumanResources));

            builder.HasKey(p => p.EmployeeId);
            builder.Property(p => p.EmployeeId).ValueGeneratedOnAdd();

            //builder.OwnsOne(x => x.Credentials, xx => { xx.HasIndex(o => new {o.Username, o.Password}).IsUnique(); });

            // builder.OwnsMany(x => x.Addresses, xx =>
            // {
            //     xx.WithOwner().HasForeignKey("EmployeeId");
            //     xx.Property<Guid>("Id");
            //     xx.HasKey("Id");
            // });

            builder.OwnsMany(x => x.Contacts);

            // builder.OwnsMany(x => x.Claims, xx =>
            // {
            //     xx.Property(o => o.GrantedTs).HasDefaultValue(DateTime.UtcNow).ValueGeneratedOnAdd();
            //     xx.HasOne(o => o.GrantedBy);
            //     xx.HasOne(o => o.RevokedBy);
            // });

            builder.OwnsMany(x => x.Identities);
        }
    }
}