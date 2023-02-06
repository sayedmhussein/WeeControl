using System.Threading;
using System.Threading.Tasks;

namespace WeeControl.Core.Domain.Interfaces;

public interface IDbContext
{
    int SaveChanges();
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}