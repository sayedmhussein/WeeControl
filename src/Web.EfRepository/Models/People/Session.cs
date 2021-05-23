using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.Shared.Library.Dbos;

namespace MySystem.Web.EfRepository.Models.People
{
    public static class EmployeeSession
    {
        static internal void CreateSessionModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeSessionDbo>().ToTable(nameof(EmployeeSession), nameof(People));
            modelBuilder.Entity<EmployeeSessionDbo>().HasIndex(x => new { x.EmployeeId, x.DeviceId, x.TerminationTs }).IsUnique(false);
            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<EmployeeSessionDbo>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<EmployeeSessionDbo>().Property(p => p.Id).ValueGeneratedOnAdd();
            }

            modelBuilder.Entity<EmployeeSessionDbo>().Property(p => p.CreationTs).HasDefaultValue(DateTime.UtcNow);
        }
    }
}
