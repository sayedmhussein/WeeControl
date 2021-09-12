﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Domain.EntityGroup.EmployeeSchema;
using WeeControl.Backend.Domain.EntityGroup.Territory;

namespace WeeControl.Backend.Domain.Interfaces
{
    public interface IMySystemDbContext
    {
        //Territory Schema
        DbSet<TerritoryDbo> Territories { get; set; }

        //Employee Schema
        DbSet<EmployeeDbo> Employees { get; set; }
        DbSet<EmployeeClaimDbo> EmployeeClaims { get; set; }
        DbSet<EmployeeIdentityDbo> EmployeeIdentities { get; set; }
        //
        DbSet<EmployeeSessionDbo> EmployeeSessions { get; set; }
        DbSet<EmployeeSessionLogDbo> EmployeeSessionLogs { get; set; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
