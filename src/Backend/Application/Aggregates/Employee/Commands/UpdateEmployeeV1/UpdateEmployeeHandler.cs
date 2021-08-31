//using System.Threading;
//using System.Threading.Tasks;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using MySystem.Application.Common.Exceptions;
//using MySystem.Application.Common.Interfaces;
//using MySystem.Domain.EntityDbo.EmployeeSchema;
//using MySystem.Domain.Extensions;
//using MySystem.SharedKernel.Entities.Employee.V1Dto;
//using MySystem.SharedKernel.Entities.Public.V1Dto;

//namespace WeeControl.Server.Application.Aggregates.Employee.Commands.UpdateEmployeeV1
//{
//    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, ResponseDto<EmployeeDto>>
//    {
//        private readonly IMySystemDbContext context;

//        public UpdateEmployeeHandler(IMySystemDbContext context)
//        {
//            this.context = context;
//        }

//        public async Task<ResponseDto<EmployeeDto>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
//        {
//            var entity = await context.Employees
//                    .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

//            if (entity == null)
//            {
//                throw new NotFoundException(nameof(request), request.Id);
//            }

//            await context.SaveChangesAsync(cancellationToken);

//            var entity_ = entity.ToDto<EmployeeDbo, EmployeeDto>();

//            return new ResponseDto<EmployeeDto>(entity_);
//        }
//    }
//}
