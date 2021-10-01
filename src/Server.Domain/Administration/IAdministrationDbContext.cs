using Microsoft.EntityFrameworkCore;
using WeeControl.Server.Domain.Administration.Entities;
using WeeControl.Server.Domain.Common.Interfaces;

namespace WeeControl.Server.Domain.Administration
{
    public interface IAdministrationDbContext : IDbContext
    {
        DbSet<Territory> Territories { get; set; }
    }
}