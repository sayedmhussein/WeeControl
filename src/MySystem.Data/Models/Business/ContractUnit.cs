using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.Data.Models.Component;

namespace MySystem.Data.Models.Business
{
    [Table(nameof(ContractUnit), Schema = nameof(Business))]
    [Index(nameof(ContractId), nameof(UnitId), IsUnique = false)]
    [Comment("-")]
    public class ContractUnit
    {
        [Key]
        public Guid ContractUnitId { get; set; }

        public Guid ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public Guid UnitId { get; set; }
        public virtual Unit Unit { get; set; }

        public DateTime ActivationTs { get; set; }

        public DateTime? CancellationTs { get; set; }

        #region ef_functions
        //static internal List<Contract> GetContractList(Guid officeid, Guid salesid)
        //{
        //    return new()
        //    {
        //        new Contract() { ContractType = 1, ContractNo = "88L12345", ContractName = "This is Contract Name", OfficeId = officeid, SalesId = salesid }
        //    };
        //}

        static internal void CreateContractUnitModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<ContractUnit>()
                .Property(p => p.ContractUnitId)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<ContractUnit>().Property(p => p.ContractUnitId).ValueGeneratedOnAdd();
            }

            modelBuilder.Entity<ContractUnit>().Property(p => p.ActivationTs).HasDefaultValue(DateTime.Now);
        }
        #endregion
    }
}
