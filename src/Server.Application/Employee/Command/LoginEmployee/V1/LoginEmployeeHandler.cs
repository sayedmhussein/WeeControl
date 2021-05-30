using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MySystem.Application.Common.Interfaces;
using MySystem.SharedKernel.Dto.V1;
using MySystem.SharedKernel.ExtensionMethod;

namespace Application.Employee.Command.LoginEmployee.V1
{
    public class LoginEmployeeHandler : IRequestHandler<LoginEmployeeCommand, ResponseDto<string>>
    {
        private readonly IMySystemDbContext context;
        private readonly IJwtService jwtService;

        public LoginEmployeeHandler(IMySystemDbContext context, IJwtService jwtService)
        {
            this.context = context;
            this.jwtService = jwtService;
        }

        public async Task<ResponseDto<string>> Handle(LoginEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException();
            }
            else if (request.Payload == null || string.IsNullOrWhiteSpace(request.DeviceId) || request.Payload.IsValid() == false)
            {
                throw new BadRequestException();
            }

            var employee = await context.Employees.FirstOrDefaultAsync(x => x.Username == request.Payload.Username && x.Password == request.Payload.Password && x.AccountLockArgument == null, cancellationToken);
            if (employee == null)
            {
                throw new NotFoundException("Username or password are not matching");
            }

            var session = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.Employee == employee && x.TerminationTs == null, cancellationToken);
            if (session == null)
            {
                session = new ();
                session.EmployeeId = employee.Id;
                session.DeviceId = request.DeviceId;
                await context.EmployeeSessions.AddAsync(session, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            var storedClaims = await context.EmployeeClaims.Where(x => x.Employee == employee && x.RevokedTs == null).ToListAsync(cancellationToken);
            var claims = new List<Claim>();

            storedClaims.ForEach(x => claims.Add(new Claim(x.ClaimType, x.ClaimValue)));

            var token = jwtService.GenerateJwtToken(claims, "issuer", DateTime.UtcNow.AddMinutes(5));

            return new ResponseDto<string>(token);
        }
    }
}
