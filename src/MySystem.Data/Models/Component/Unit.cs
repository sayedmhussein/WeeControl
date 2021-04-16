using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MySystem.Data.Models.Component
{
    [Table(nameof(Unit), Schema = nameof(Component))]
    [Index(nameof(UnitNo), IsUnique = true)]
    [Index(nameof(UnitType), IsUnique = false)]
    [Comment("-")]
    public class Unit
    {
        #region ef_functions
        static internal List<Unit> GetUnitList()
        {
            return new()
            {
                new Unit() { UnitNo = "88NB1234", UnitType = Types.Elevator },
                new Unit() { UnitNo = "88NE1234", UnitType = Types.Elevator },
                new Unit() { UnitNo = "88KE1234", UnitType = Types.Dumbwaiter },
                new Unit() { UnitNo = "88KC1234", UnitType = Types.Travellator },
                new Unit() { UnitNo = "88SH1234", UnitType = Types.Elevator },
                new Unit() { UnitNo = "88MT1234", UnitType = Types.Escalator }
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

        [Key]
        public Guid UnitId { get; set; }

        [StringLength(10, ErrorMessage = "Always follow conventions ie. 88NB1234.")]
        public string UnitNo { get; set; }

        public Types UnitType { get; set; }

        public enum Types { Elevator, Escalator, Travellator, Dumbwaiter, Platform, Other }
    }
}
