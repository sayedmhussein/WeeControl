using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MySystem.Application.Common.Exceptions;
using MySystem.Application.Common.Interfaces;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.Application.Employee.Query.GetEmployeeClaims
{
    public class GetEmployeeClaimsV1Handler : IRequestHandler<GetEmployeeClaimsV1Query, IEnumerable<Claim>>
    {
        private readonly IMySystemDbContext context;
        private readonly ICurrentUserInfo currentUser;
        private readonly ISharedValues sharedValues;
        private readonly IMediator mediator;

        public GetEmployeeClaimsV1Handler(IMySystemDbContext context, ICurrentUserInfo currentUser, ISharedValues sharedValues, IMediator mediator)
        {
            this.context = context ?? throw new ArgumentNullException();
            this.currentUser = currentUser ?? throw new ArgumentNullException();
            this.sharedValues = sharedValues ?? throw new ArgumentNullException();
            this.mediator = mediator ?? throw new ArgumentNullException();
        }

        public async Task<IEnumerable<Claim>> Handle(GetEmployeeClaimsV1Query request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException();

            if (request.EmployeeId == null)
            {
                if (string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Password) && string.IsNullOrEmpty(request.Device) == false)
                {
                    return await GetClaimsByRefreshingToken(request.Device, cancellationToken);
                }
                else if (string.IsNullOrWhiteSpace(request.Username) == false && string.IsNullOrWhiteSpace(request.Password) == false && string.IsNullOrWhiteSpace(request.Device) == false)
                {
                    return await GetClaimsByUsernameAndPassword(request.Username, request.Password, request.Device, cancellationToken);
                }
                else
                {
                    throw new BadRequestException("Must provide any of query properties!");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(request.Username) != true || string.IsNullOrWhiteSpace(request.Username) != true || string.IsNullOrWhiteSpace(request.Username) != true)
                {
                    throw new BadRequestException("Either employee id or credentials or device!");
                }

                throw new NotImplementedException();
            }
        }

        private async Task<Guid> GetEmployeeSession(Guid employeeid, string device, CancellationToken cancellationToken)
        {
            var session = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.EmployeeId == employeeid && x.TerminationTs == null, cancellationToken);
            if (session == null)
            {
                session = new();
                session.EmployeeId = employeeid;
                session.DeviceId = device;
                await context.EmployeeSessions.AddAsync(session, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            return session.Id;
        }

        private async Task<IEnumerable<Claim>> GetClaimsByUsernameAndPassword(string username, string password, string device, CancellationToken cancellationToken)
        {
            var employee = await context.Employees.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
            if (employee == null)
            {
                throw new NotFoundException("", "");
            }
            else if (employee.AccountLockArgument != null)
            {
                throw new NotAllowedException("");
            }
            else
            {
                var session = GetEmployeeSession(employee.Id, device, cancellationToken);
                var claims = new List<Claim>()
                {
                    new Claim(sharedValues.ClaimType[ClaimTypeEnum.Session], session.ToString())
                };
                return claims;
            }
        }

        private async Task<IEnumerable<Claim>> GetClaimsByRefreshingToken(string device, CancellationToken cancellationToken)
        {
            if (currentUser.SessionId == null)
            {
                throw new NotAllowedException("");
            }

            var session = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.Id == currentUser.SessionId && x.DeviceId == device && x.TerminationTs == null);
            if (session == null)
            {
                throw new NotAllowedException("");
            }

            var employee = await context.Employees.FirstOrDefaultAsync(x => x.Id == session.EmployeeId && x.AccountLockArgument == null);
            if (employee == null)
            {
                throw new NotAllowedException("");
            }

            var claims = new List<Claim>()
            {
                 new Claim(sharedValues.ClaimType[ClaimTypeEnum.Session], session.ToString())
            };

            var employeeClaims = await context.EmployeeClaims.Where(x => x.EmployeeId == employee.Id && x.RevokedTs == null).ToListAsync(cancellationToken);
            employeeClaims.ForEach(x => claims.Add(new Claim(x.ClaimType, x.ClaimValue)));

            return claims;
        }
    }
}
