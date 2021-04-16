using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.Data.Models.Basic;
using MySystem.Data.Models.People;

namespace MySystem.Data.Models.Business
{
    [Table(nameof(Contract), Schema = nameof(Business))]
    [Index(nameof(ContractNo), IsUnique = true)]
    [Index(nameof(ContractType), IsUnique = false)]
    [Comment("-")]
    public class Contract
    {
        [Key]
        public Guid ContractId { get; set; }

        public Guid? ParentId { get; set; }
        public virtual Contract Parent { get; set; }

        [Required]
        [StringLength(10)]
        public string ContractNo { get; set; }

        [Required]
        public int ContractType { get; set; }

        [Required]
        [StringLength(255)]
        public string ContractName { get; set; }

        public DateTime CreationTs { get; set; }

        public Guid SalesId { get; set; }
        public virtual Employee Sales { get; set; }

        public Guid OfficeId { get; set; }
        public virtual Office Office { get; set; }

        #region ef_functions
        static internal List<Contract> GetContractList(Guid officeid, Guid salesid)
        {
            return new()
            {
                new Contract() { ContractType = 1, ContractNo = "88L12345", ContractName = "This is Contract Name", OfficeId = officeid, SalesId = salesid }
            };
        }

        static internal void CreateUnitModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<Contract>()
                .Property(p => p.ContractId)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<Contract>().Property(p => p.ContractId).ValueGeneratedOnAdd();
            }

            modelBuilder.Entity<Contract>().Property(p => p.CreationTs).HasDefaultValue(DateTime.Now);
        }
        #endregion
    }
}
