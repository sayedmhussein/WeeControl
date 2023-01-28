using System.Threading;
using System.Threading.Tasks;

namespace WeeControl.Core.Application.Interfaces;

public interface IDbContext
{
    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}