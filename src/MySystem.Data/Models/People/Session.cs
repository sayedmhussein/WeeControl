using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sayed.MySystem.ServerData.Models.People
{
    [Table(nameof(Session), Schema = nameof(People))]
    [Index(nameof(EmployeeId), nameof(DeviceId), nameof(TerminationTs), IsUnique = false)]
    [Comment("User Sessions")]
    public class Session
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public string DeviceId { get; set; }

        public DateTime CreationTs { get; set; }

        public DateTime? TerminationTs { get; set; }

        #region ef_functions
        static internal void CreateSessionModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<Session>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<Session>().Property(p => p.Id).ValueGeneratedOnAdd();
            }

            modelBuilder.Entity<Session>().Property(p => p.CreationTs).HasDefaultValue(DateTime.UtcNow);
        }
        #endregion
    }
}
