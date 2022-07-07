using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Application.Interfaces;
using System;

namespace WeeControl.Application.Contexts.System.Notifications;

public class MediatorLogCommand : IRequest
{
    public class MediatorLogHandler : IRequestHandler<MediatorLogCommand>
    {
        private readonly IEssentialDbContext essentialDbContext;

        public MediatorLogHandler(IEssentialDbContext essentialDbContext)
        {
            this.essentialDbContext = essentialDbContext;
        }
        
        public Task<Unit> Handle(MediatorLogCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}