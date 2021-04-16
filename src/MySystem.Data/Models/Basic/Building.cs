using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MySystem.Data.Models.Basic
{
    [Table(nameof(Building), Schema = nameof(Basic))]
    [Comment("Offices of corporate.")]
    public class Building
    {
        [Key]
        public Guid BuildingId { get; set; }

        public string BuildingName { get; set; }

        public string CountryId { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public Building()
        {
        }

        public Building(string name, string country) : this()
        {
            BuildingName = name;
            CountryId = country;
        }

        #region ef_functions
        static internal List<Building> GetOfficeList()
        {
            var ho = new Building() { BuildingId = Guid.NewGuid(), CountryId = "SAU", BuildingName = "Saudi Arabia Head Office" };
            return new()
            {
                ho,
                new Building("Jeddah", "SAU")
            };
        }

        static internal void CreateBuildingModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<Building>()
                .Property(p => p.BuildingId)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<Building>().Property(p => p.BuildingId).ValueGeneratedOnAdd();
            }
        }
        #endregion
    }
}
