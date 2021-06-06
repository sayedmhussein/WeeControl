using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MySystem.Application.Common.Interfaces;

namespace MySystem.Application.Employee.Command.TerminateSession.V1
{
    public class TerminateEmployeeHandler : IRequestHandler<TerminateSessionCommand>
    {
        private readonly IMySystemDbContext context;

        public TerminateEmployeeHandler(IMySystemDbContext context)
        {
            this.context = context;
        }

        public Task<Unit> Handle(TerminateSessionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
