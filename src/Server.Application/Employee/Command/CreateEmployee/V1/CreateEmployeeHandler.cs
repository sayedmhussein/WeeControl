using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MySystem.Application.Common.Interfaces;
using MySystem.Application.Employee.Command.CreateEmployee.V1;
using MySystem.Domain.EntityDbo;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using MySystem.Domain.Extensions;
using MySystem.SharedKernel.Dto.V1;

namespace Application.Employee.Command.CreateEmployee.V1
{
    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, ResponseDto<EmployeeDto>>
    {
        private readonly IMySystemDbContext context;
        private readonly IMediator mediator;

        public CreateEmployeeHandler(IMySystemDbContext context, IMediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<ResponseDto<EmployeeDto>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = request.Payload.ToDbo<EmployeeDto, EmployeeDbo>();
            await context.Employees.AddAsync(entity);
            await context.SaveChangesAsync(cancellationToken);

            await mediator.Publish(new EmployeeCreated { EmployeeId = entity.Id }, cancellationToken);

            var entity_ = entity.ToDto<EmployeeDbo, EmployeeDto>();

            return new ResponseDto<EmployeeDto>(entity_);
        }
    }
}
