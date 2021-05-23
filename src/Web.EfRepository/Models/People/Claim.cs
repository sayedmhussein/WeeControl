using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.Shared.Library.Dbos;

namespace MySystem.Web.EfRepository.Models.People
{
    public static class EmployeeClaim
    {
        static internal void CreateClaimModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeClaimDbo>().ToTable(nameof(EmployeeClaim), nameof(People));
            modelBuilder.Entity<EmployeeClaimDbo>().HasIndex(x => x.EmployeeId).IsUnique(false);

            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<EmployeeClaimDbo>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<EmployeeClaimDbo>().Property(p => p.Id).ValueGeneratedOnAdd();
            }

            modelBuilder.Entity<EmployeeClaimDbo>().Property(p => p.GrantedTs).HasDefaultValue(DateTime.UtcNow);
        }

    }
}
