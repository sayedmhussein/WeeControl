using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MySystem.Data.Models.Basic
{
    [Table(nameof(Office), Schema = nameof(Basic))]
    [Index(nameof(CountryId), nameof(OfficeName), IsUnique = true)]
    [Comment("Offices of corporate.")]
    public class Office
    {
        [Key]
        [Column(nameof(OfficeId))]
        [Display(Name = "ID")]
        public Guid OfficeId { get; set; }

        [Comment("Local inhertance from this table primay key.")]
        public Guid? ParentId { get; set; }
        public virtual Office Parent { get; set; }

        [Required]
        [StringLength(3, ErrorMessage = "Only ISO 3166-1 alpha-3 Country Codes.")]
        public string CountryId { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "Office name must not exceed 45 character.")]
        public string OfficeName { get; set; }

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
            var ho = new Office() { OfficeId = Guid.NewGuid(), CountryId = "SAU", OfficeName = "Saudi Arabia Head Office" };
            return new()
            {
                ho,
                new Office("Jeddah", "SAU", ho.OfficeId),
                new Office("Riyadh", "SAU", ho.OfficeId),
                new Office("Mecca", "SAU", ho.OfficeId)
            };
        }

        static internal void CreateOfficeModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<Office>()
                .Property(p => p.OfficeId)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<Office>().Property(p => p.OfficeId).ValueGeneratedOnAdd();
            }

            modelBuilder.Entity<Office>().HasOne(e => e.Parent).WithMany();
        }
        #endregion
    }
}
