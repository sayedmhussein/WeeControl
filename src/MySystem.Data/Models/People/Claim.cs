using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sayed.MySystem.ServerData.Models.People
{
    [Table(nameof(Claim), Schema = nameof(People))]
    [Index(nameof(PersonId), IsUnique = false)]
    [Comment("User Sessions")]
    public class Claim
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PersonId { get; set; }
        public virtual Person Person { get; set; }

        public DateTime GrantedTs { get; set; }

        public DateTime? RevokedTs { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        #region ef_functions
        static internal void CreateClaimModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<Claim>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<Claim>().Property(p => p.Id).ValueGeneratedOnAdd();
            }

            modelBuilder.Entity<Claim>().Property(p => p.GrantedTs).HasDefaultValue(DateTime.UtcNow);
        }
        #endregion
    }
}
