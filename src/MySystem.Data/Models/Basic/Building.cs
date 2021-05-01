using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.SharedDto.BaseEntities;

namespace MySystem.ServerData.Models.Basic
{
    [Table(nameof(Building), Schema = nameof(Basic))]
    [Comment("Offices of corporate.")]
    public class Building : BuildingBase
    {
        [Key]
        public Guid Id { get; set; }

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
            var ho = new Building() { Id = Guid.NewGuid(), CountryId = "SAU", BuildingName = "Saudi Arabia Head Office" };
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
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<Building>().Property(p => p.Id).ValueGeneratedOnAdd();
            }
        }
        #endregion
    }
}
