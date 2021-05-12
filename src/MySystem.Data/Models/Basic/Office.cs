using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sayed.MySystem.SharedDto.BaseEntities;

namespace Sayed.MySystem.ServerData.Models.Basic
{
    [Table(nameof(Office), Schema = nameof(Basic))]
    [Index(nameof(CountryId), nameof(OfficeName), IsUnique = true)]
    [Comment("Offices of corporate.")]
    public class Office : OfficeBase
    {
        [Key]
        [Column(nameof(Id))]
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Comment("Local inhertance from this table primay key.")]
        public Guid? ParentId { get; set; }
        public virtual Office Parent { get; set; }

        #region constructors
        public Office()
        {
        }

        public Office(string name, string country) : this()
        {
            OfficeName = name;
            CountryId = country;
        }

        public Office(string name, string country, Guid parentid) : this(name, country)
        {
            ParentId = parentid;
        }
        #endregion

        #region ef_functions
        static internal List<Office> GetOfficeList()
        {
            var ho = new Office() { Id = Guid.Parse("12345678-abcd-1234-abcd-123456789abc"), CountryId = "SAU", OfficeName = "Saudi Arabia Head Office" };
            return new()
            {
                ho,
                new Office("Jeddah", "SAU", ho.Id),
                new Office("Riyadh", "SAU", ho.Id),
                new Office("Mecca", "SAU", ho.Id)
            };
        }

        static internal void CreateOfficeModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<Office>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<Office>().Property(p => p.Id).ValueGeneratedOnAdd();
            }

            modelBuilder.Entity<Office>().HasOne(e => e.Parent).WithMany();
        }
        #endregion
    }
}
