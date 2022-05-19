using System.Threading;
using System.Threading.Tasks;

namespace WeeControl.Domain.Interfaces;

public interface IDbContext
{
    int SaveChanges();
        
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// DANGER danger !!! ONLY FOR DEVELOPMET
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ResetDatabaseAsync(CancellationToken cancellationToken);
}