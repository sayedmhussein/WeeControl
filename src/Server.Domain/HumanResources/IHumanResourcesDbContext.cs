using Microsoft.EntityFrameworkCore;
using WeeControl.Server.Domain.Common.Interfaces;
using WeeControl.Server.Domain.HumanResources.Entities;

namespace WeeControl.Server.Domain.HumanResources
{
    public interface IHumanResourcesDbContext : IDbContext
    { 
        DbSet<Employee> Employees { get; set; }
    }
}