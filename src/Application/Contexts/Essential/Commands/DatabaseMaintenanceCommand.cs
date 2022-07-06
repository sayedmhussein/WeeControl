using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Application.Interfaces;

namespace WeeControl.Application.Contexts.Essential.Commands;

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