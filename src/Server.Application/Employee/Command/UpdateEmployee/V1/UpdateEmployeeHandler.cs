using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MySystem.Application.Common.Interfaces;
using MySystem.Domain.EntityDbo;
using MySystem.Domain.Extensions;
using MySystem.SharedKernel.Dto.V1;

namespace Application.Employee.Command.UpdateEmployee.V1
{
    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, ResponseDto<EmployeeDto>>
    {
        private readonly IMySystemDbContext context;

        public UpdateEmployeeHandler(IMySystemDbContext context)
        {
            this.context = context;
        }

        public async Task<ResponseDto<EmployeeDto>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Employees
                    .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(request), request.Id);
            }

            await context.SaveChangesAsync(cancellationToken);

            var entity_ = entity.ToDto<EmployeeDbo, EmployeeDto>();

            return new ResponseDto<EmployeeDto>(entity_);
        }
    }
}
