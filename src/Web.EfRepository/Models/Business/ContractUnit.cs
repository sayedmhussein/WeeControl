using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MySystem.Shared.Library.Dbos;
using MySystem.Web.EfRepository.Models.Component;

namespace MySystem.Web.EfRepository.Models.Business
{
    internal static class ContractUnit
    {
        static internal void CreateContractUnitModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractUnitDbo>().ToTable(nameof(ContractUnit), nameof(Business));
            modelBuilder.Entity<ContractUnitDbo>().HasIndex(x => new { x.ContractId, x.UnitId }).IsUnique(false);

            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<ContractUnitDbo>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<ContractUnitDbo>().Property(p => p.Id).ValueGeneratedOnAdd();
            }

            modelBuilder.Entity<ContractUnitDbo>().Property(p => p.ActivationTs).HasDefaultValue(DateTime.Now);
        }



        #region ef_functions
        static internal List<ContractUnitDbo> GetContractUnitList(Guid contractId, Guid unitId)
        {
            return new()
            {
                new ContractUnitDbo() { ContractId = contractId, UnitId = unitId }
            };
        }

        
        #endregion
    }
}
