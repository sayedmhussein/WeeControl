using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.Shared.Library.Dbos;

namespace MySystem.Web.EfRepository.Models.People
{
    [Table(nameof(SessionActivity), Schema = nameof(People))]
    [Index(nameof(SessionId), IsUnique = false)]
    public class SessionActivity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid SessionId { get; set; }
        public EmployeeSessionDbo Session { get; set; }

        public DateTime ActivityTs { get; set; }

        public string Details { get; set; }

       


        #region ef_functions
        static internal void CreateModelBuilder(DbContext dbContext, ModelBuilder modelBuilder)
        {
            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<SessionActivity>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<SessionActivity>().Property(p => p.Id).ValueGeneratedOnAdd();
            }

            modelBuilder.Entity<SessionActivity>().Property(p => p.ActivityTs).HasDefaultValue(DateTime.UtcNow);
        }
        #endregion
    }
}
