using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.Entities;

namespace WeeControl.Backend.Persistence.BoundedContexts.HumanResources.Configurations
{
    public class SessionEntityTypeConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(nameof(Session), nameof(Employee));

            builder.Property(p => p.SessionId).ValueGeneratedOnAdd();
            builder.Property(p => p.CreationTs).HasDefaultValue(DateTime.UtcNow);

            builder.OwnsMany(x => x.Logs, xx =>
            {
                xx.WithOwner().HasForeignKey();
                xx.Property(o => o.LogTs).HasDefaultValue(DateTime.UtcNow);
            });

        }
    }
}