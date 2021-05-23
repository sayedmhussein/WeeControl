using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.Shared.Library.Dbos;

namespace MySystem.Web.EfRepository.Models.Basic
{
    public static class Building
    {
        internal static void CreateModelBuilder(DbContext dbContext, ModelBuilder modelBuilder)
        {
            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<BuildingDbo>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<BuildingDbo>().Property(p => p.Id).ValueGeneratedOnAdd();
            }

            modelBuilder.Entity<BuildingDbo>().ToTable(nameof(Building), nameof(Basic));
            modelBuilder.Entity<BuildingDbo>().HasComment("Offices of corporate.");
        }



        #region ef_functions
        static internal List<BuildingDbo> GetOfficeList()
        {
            var ho = new BuildingDbo() { Id = Guid.NewGuid(), CountryId = "SAU", BuildingName = "Saudi Arabia Head Office" };
            return new List<BuildingDbo>
            {
                ho,
                new BuildingDbo("Jeddah", "SAU")
            };
        }
        #endregion
    }
}
