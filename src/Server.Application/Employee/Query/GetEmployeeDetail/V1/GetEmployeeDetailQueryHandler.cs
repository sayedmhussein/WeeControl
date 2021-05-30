using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MySystem.Application.Common.Interfaces;
using MySystem.Domain.EntityDbo;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using MySystem.Domain.Extensions;
using MySystem.SharedKernel.Dto.V1;

namespace Application.Employee.Query.GetEmployeeDetail.V1
{
    public class GetEmployeeDetailQueryHandler : IRequestHandler<GetEmployeeDetailQuery, EmployeeDto>
    {
        private readonly IMySystemDbContext context;

        public GetEmployeeDetailQueryHandler(IMySystemDbContext context)
        {
            this.context = context;
        }


        public async Task<EmployeeDto> Handle(GetEmployeeDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await context.Employees
                .FindAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                //throw new NotFoundException(nameof(Customer), request.Id);
            }

            return entity.ToDto<EmployeeDbo, EmployeeDto>();
        }
    }
}
