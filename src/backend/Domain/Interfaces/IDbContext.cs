using System.Threading;
using System.Threading.Tasks;

namespace WeeControl.ApiApp.Domain.Interfaces;

public interface IDbContext
{
    int SaveChanges();
        
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}