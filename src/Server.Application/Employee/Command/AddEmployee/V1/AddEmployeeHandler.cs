using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MySystem.Application.Common.Exceptions;
using MySystem.Application.Common.Interfaces;
using MySystem.Application.Employee.Command.AddEmployee.V1;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using MySystem.Domain.Extensions;
using MySystem.SharedKernel.Entities.Public.V1Dto;
using MySystem.SharedKernel.Entities.Employee.V1Dto;

namespace Application.Employee.Command.CreateEmployee.V1
{
    public class AddEmployeeHandler : IRequestHandler<AddEmployeeCommand, ResponseDto<EmployeeDto>>
    {
        private readonly IMySystemDbContext context;
        private readonly ICurrentUserInfo currentUser;
        //private readonly IMediator mediator;

        public AddEmployeeHandler(IMySystemDbContext context, ICurrentUserInfo currentUser)//, IMediator mediator)
        {
            this.context = context;
            this.currentUser = currentUser;
            //this.mediator = mediator;
        }

        public async Task<ResponseDto<EmployeeDto>> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = request.Payload.ToDbo<EmployeeDto, EmployeeDbo>();
            await context.Employees.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            //await mediator.Publish(new EmployeeCreated { EmployeeId = entity.Id }, cancellationToken);

            var entity_ = entity.ToDto<EmployeeDbo, EmployeeDto>();

            return new ResponseDto<EmployeeDto>(entity_);
        }
    }
}
