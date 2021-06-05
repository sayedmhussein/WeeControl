using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MySystem.Application.Common.Exceptions;
using MySystem.Application.Common.Interfaces;
using MySystem.Application.Employee.Command.AddEmployee.V1;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using MySystem.Domain.Extensions;
using MySystem.SharedKernel.Entities.Employee.V1Dto;
using MySystem.SharedKernel.Entities.Public.Constants;
using MySystem.SharedKernel.Entities.Public.V1Dto;
using MySystem.SharedKernel.ExtensionMethods;

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
            if (request.Payload.IsValid() == false)
            {
                throw new ValidationException(request.Payload.GetErrorMessages());
            }

            var claimTags = currentUser.Claims.FirstOrDefault(c => c.Type == Claims.Types[Claims.ClaimType.HumanResources])?.Value;
            if (claimTags == null || claimTags.Contains(Claims.Tags[Claims.ClaimTag.Add]) == false)
            {
                throw new NotAllowedException("User not authorizedd to add employee.");
            }

            if (currentUser.TerritoriesId.Contains(request.Payload.TerritoryId) == false)
            {
                throw new NotAllowedException("User not authorizedd to add employee in selected office.");
            }

            var entity = request.Payload.ToDbo<EmployeeDto, EmployeeDbo>();
            await context.Employees.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            //await mediator.Publish(new EmployeeCreated { EmployeeId = entity.Id }, cancellationToken);

            var entity_ = entity.ToDto<EmployeeDbo, EmployeeDto>();

            return new ResponseDto<EmployeeDto>(entity_);
        }
    }
}
