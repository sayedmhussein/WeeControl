using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MySystem.Application.Common.Interfaces;
using MySystem.SharedKernel.Definition;
using MySystem.SharedKernel.Dto.V1;

namespace Application.Employee.Command.RefreshEmployeeToken.V1
{
    public class RefreshEmployeeTokenHandler : IRequestHandler<RefreshEmployeeTokenCommand, ResponseDto<string>>
    {
        private readonly IMySystemDbContext context;
        private readonly IJwtService jwtService;

        public RefreshEmployeeTokenHandler(IMySystemDbContext context, IJwtService jwtService)
        {
            this.context = context;
            this.jwtService = jwtService;
        }

        public async Task<ResponseDto<string>> Handle(RefreshEmployeeTokenCommand request, CancellationToken cancellationToken)
        {
            var claims_ = jwtService.GetClaims(request.Token);
            var session = claims_.Claims.FirstOrDefault(x => x.Type == UserClaim.Session)?.Value;
            if (session == null)
            {
                return new ResponseDto<string>(string.Empty);
            }

            var tryParse = Guid.TryParse(session, out Guid sessionid);
            if (tryParse == false)
            {
                return new ResponseDto<string>(string.Empty);
                //Todo: Log here please
            }

            var employee = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.Id == sessionid && x.TerminationTs == null);
            if (employee == null)
            {
                return new ResponseDto<string>(string.Empty);
                //Todo: Log here please
            }

            var isAllowedToLogin = await context.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id && x.AccountLockArgument == null);
            if (isAllowedToLogin == null)
            {
                return new ResponseDto<string>(string.Empty);
                //Todo: Log here please
            }

            //Todo: Log This Activity

            var storedClaims = await context.EmployeeClaims.Where(x => x.Employee == employee.Employee && x.RevokedTs == null).ToListAsync(cancellationToken);
            var claims = new List<Claim>();

            storedClaims.ForEach(x => claims.Add(new Claim(x.ClaimType, x.ClaimValue)));

            var token = jwtService.GenerateJwtToken(claims, "issuer", DateTime.UtcNow.AddMinutes(5));
            return new ResponseDto<string>(token);
        }
    }
}
