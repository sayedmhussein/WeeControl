﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySystem.Domain.EntityDbo.ContractSchema;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using MySystem.Domain.EntityDbo.PublicSchema;
using MySystem.Domain.EntityDbo.UnitSchema;

namespace MySystem.Application.Common.Interfaces
{
    public interface IMySystemDbContext
    {
        //Basic Schema
        DbSet<TerritoryDbo> Territories { get; set; }
        DbSet<BuildingDbo> Buildings { get; set; }

        //Employee Schema
        DbSet<EmployeeDbo> Employees { get; set; }
        DbSet<EmployeeClaimDbo> EmployeeClaims { get; set; }
        DbSet<EmployeeIdentityDbo> EmployeeIdentities { get; set; }
        //
        DbSet<EmployeeSessionDbo> EmployeeSessions { get; set; }
        DbSet<EmployeeSessionLogDbo> EmployeeSessionLogs { get; set; }


        //Component Schema
        DbSet<UnitDbo> Units { get; set; }

        //Business Schema
        DbSet<ContractDbo> Contracts { get; set; }
        DbSet<ContractUnitDbo> ContractUnits { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
