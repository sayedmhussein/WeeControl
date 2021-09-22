using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Domain.EntityGroups.Employee;
using WeeControl.Backend.Domain.EntityGroups.Territory;

namespace WeeControl.Backend.Domain.Common.Interfaces
{
    public interface IMySystemDbContext : IDbContext
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
        
    }
}
