using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MySystem.Domain.EntityDbo;
using MySystem.Domain.EntityDbo.ContractSchema;

namespace MySystem.Persistence.Infrastructure.EfRepository.Models.Business
{
    internal static class ContractUnit
    {
        [Obsolete]
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
