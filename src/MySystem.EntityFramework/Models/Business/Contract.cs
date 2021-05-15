using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Sayed.MySystem.EntityFramework.Models.Basic;
using Sayed.MySystem.EntityFramework.Models.People;
using Sayed.MySystem.Shared.Dbos;
using Sayed.MySystem.Shared.Entities;

namespace Sayed.MySystem.EntityFramework.Models.Business
{
    internal static class Contract
    {
        static internal void CreateModelBuilder(DbContext dbContext, ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContractDbo>().ToTable(nameof(Contract), nameof(Business));
            //modelBuilder.Entity<ContractDbo>().HasOne(x => x.Parent);

            if (dbContext.Database.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp")
                .Entity<ContractDbo>()
                .Property(p => p.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
            }
            else
            {
                modelBuilder.Entity<ContractDbo>().Property(p => p.Id).ValueGeneratedOnAdd();
            }

            //modelBuilder.Entity<ContractDbo>().OwnsOne(typeof(ContractBase), "");
            
            modelBuilder.Entity<ContractDbo>().HasComment("'_'");
            modelBuilder.Entity<ContractDbo>().Property(p => p.CreationTs).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<ContractDbo>().HasIndex(x => x.ContractNo).IsUnique(true);
            modelBuilder.Entity<ContractDbo>().HasIndex(x => x.ContractType).IsUnique(false);
        }

        static internal List<ContractDbo> GetContractList(Guid officeid, Guid salesid)
        {
            return new List<ContractDbo>
            {
                new ContractDbo()
                {
                    ContractType = 1, ContractNo = "88L12345", ContractName = "This is Contract Name", OfficeId = officeid, SalesId = salesid
                }
            };
        }
    }
}
