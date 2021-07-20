using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Server.Application.Common.Exceptions;
using WeeControl.Server.Domain.Interfaces;

namespace WeeControl.Server.Application.Employee.Command.DeleteEmployee.V1
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
    {
        private readonly IMySystemDbContext context;

        public DeleteEmployeeCommandHandler(IMySystemDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Employees
                .FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Employee), request.Id);
            }

            var hasSessions = context.EmployeeSessions.Any(o => o.EmployeeId == entity.Id);
            if (hasSessions)
            {
                throw new DeleteFailureException(nameof(Employee), request.Id, "There are existing sessions associated with this customer.");
            }

            context.Employees.Remove(entity);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
