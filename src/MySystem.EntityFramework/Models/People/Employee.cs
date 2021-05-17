using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sayed.MySystem.EntityFramework.Models.Basic;
using Sayed.MySystem.Shared.Dbos;
using Sayed.MySystem.Shared.Base;

namespace Sayed.MySystem.EntityFramework.Models.People
{
    public static class Employee
    {
        static internal void CreateModelBuilder(DbContext dbContext, ModelBuilder modelBuilder)
        {
            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<EmployeeDbo>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<EmployeeDbo>().Property(p => p.Id).ValueGeneratedOnAdd();
            }

            modelBuilder.Entity<EmployeeDbo>().Property(p => p.IsProductive).HasDefaultValue(false);

            modelBuilder.Entity<EmployeeDbo>().ToTable(nameof(Employee), nameof(People));
            modelBuilder.Entity<EmployeeDbo>().HasComment("This table inherts from Person table.");
            modelBuilder.Entity<EmployeeDbo>().HasIndex(x => x.Username).IsUnique(true);
            modelBuilder.Entity<EmployeeDbo>().HasIndex(x => x.OfficeId).IsUnique(false);
        }

        static internal List<EmployeeDbo> GetPersonList(Guid officeid)
        {
            var sayed = new EmployeeDbo() { Id = Guid.NewGuid(), Title = "Mr.", FirstName = "Sayed", LastName = "Hussein", Gender = "m", OfficeId = officeid, Username = "sayed", Password = "sayed" };
            var hatem = new EmployeeDbo() { Id = Guid.NewGuid(), Title = "Mr.", FirstName = "Hatem", LastName = "Nagaty", Gender = "m", OfficeId = officeid, Username = "hatem", Password = "hatem" };
            return new()
            {
                sayed,
                hatem,
                new EmployeeDbo() { FirstName = "Yasser", LastName = "Gamal", OfficeId = officeid, Supervisor = sayed, Username = "sayed1", Password = "sayed" }, //
                new EmployeeDbo() { FirstName = "Yasser", LastName = "Gamal", OfficeId = officeid, SupervisorId = sayed.Id, Username = "sayed2", Password = "sayed" } //, SupervisorId = sayed.Id
            };
        }
    }
}
