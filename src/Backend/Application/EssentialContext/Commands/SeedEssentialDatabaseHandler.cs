using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace WeeControl.Backend.Application.EssentialContext.Commands;

public class SeedEssentialDatabaseHandler : IRequestHandler<SeedEssentialDatabaseCommand>
{
    private readonly IEssentialDbContext context;

    public SeedEssentialDatabaseHandler(IEssentialDbContext context)
    {
        this.context = context;
    }
    
    public Task<Unit> Handle(SeedEssentialDatabaseCommand request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}