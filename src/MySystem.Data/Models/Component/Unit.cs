using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.Data.Models.Basic;

namespace MySystem.Data.Models.Component
{
    [Table(nameof(Unit), Schema = nameof(Component))]
    [Index(nameof(UnitNo), IsUnique = true)]
    [Index(nameof(UnitType), IsUnique = false)]
    [Comment("-")]
    internal class Unit
    {
        [Key]
        public Guid UnitId { get; set; }

        [StringLength(10, ErrorMessage = "Always follow conventions ie. 88NB1234.")]
        public string UnitNo { get; set; }

        public Types UnitType { get; set; }

        public Guid BuildingId { get; set; }
        public virtual Building Building { get; set; }

        public enum Types { Elevator, Escalator, Travellator, Dumbwaiter, Platform, Other }

        #region ef_functions
        static internal List<Unit> GetUnitList(Guid buildingId)
        {
            return new()
            {
                new Unit() { UnitNo = "88NB1234", UnitType = Types.Elevator, BuildingId = buildingId },
                new Unit() { UnitNo = "88NE1234", UnitType = Types.Elevator, BuildingId = buildingId },
                new Unit() { UnitNo = "88KE1234", UnitType = Types.Dumbwaiter, BuildingId = buildingId },
                new Unit() { UnitNo = "88KC1234", UnitType = Types.Travellator, BuildingId = buildingId },
                new Unit() { UnitNo = "88SH1234", UnitType = Types.Elevator, BuildingId = buildingId },
                new Unit() { UnitNo = "88MT1234", UnitType = Types.Escalator, BuildingId = buildingId }
            };
        }

        static internal void CreateUnitModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<Unit>()
                .Property(p => p.UnitId)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<Unit>().Property(p => p.UnitId).ValueGeneratedOnAdd();
            }
        }
        #endregion
    }
}
