using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.Entities;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.TerritoryModule.Entities;
using WeeControl.Backend.Domain.Common.Interfaces;

namespace WeeControl.Backend.Domain.BoundedContexts.HumanResources
{
    public interface IHumanResourcesDbContext : IDbContext
    { 
        DbSet<Employee> Employees { get; set; }

        DbSet<Territory> Territories { get; set; }
    }
}