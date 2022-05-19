using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace WeeControl.Application.EssentialContext.Commands;

public class DatabaseMaintenanceCommand : IRequest
{
    public class DatabaseMaintenanceHandler : IRequestHandler<DatabaseMaintenanceCommand>
    {
        public DatabaseMaintenanceHandler(IEssentialDbContext context)
        {
            
        }
        
        public Task<Unit> Handle(DatabaseMaintenanceCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}