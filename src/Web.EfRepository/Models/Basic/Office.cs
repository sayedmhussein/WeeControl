using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.Shared.Library.Dbos;

namespace MySystem.Web.EfRepository.Models.Basic
{
    public static class Office
    {
        static internal void CreateModelBuilder(DbContext dbContext, ModelBuilder modelBuilder)
        {
            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<OfficeDbo>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<OfficeDbo>().Property(p => p.Id).ValueGeneratedOnAdd();
            }

            modelBuilder.Entity<OfficeDbo>().HasOne(e => e.Parent).WithMany();
            modelBuilder.Entity<OfficeDbo>().ToTable(nameof(Office), nameof(Basic));
            modelBuilder.Entity<OfficeDbo>().HasComment("Offices of corporate.");
            modelBuilder.Entity<OfficeDbo>().HasIndex(x => new { x.CountryId, x.OfficeName}).IsUnique(true);

            modelBuilder.Entity<OfficeDbo>().Property(x => x.ParentId).HasComment("Local inhertance from this table primay key.");
        }

        static internal List<OfficeDbo> GetOfficeList()
        {
            var ho = new OfficeDbo() { Id = Guid.Parse("12345678-abcd-1234-abcd-123456789abc"), CountryId = "SAU", OfficeName = "Saudi Arabia Head Office" };
            return new()
            {
                ho,
                new OfficeDbo("Jeddah", "SAU", ho.Id),
                new OfficeDbo("Riyadh", "SAU", ho.Id),
                new OfficeDbo("Mecca", "SAU", ho.Id)
            };
        }
    }
}
