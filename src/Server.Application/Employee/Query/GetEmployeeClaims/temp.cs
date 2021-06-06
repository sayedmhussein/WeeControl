//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading;
//using System.Threading.Tasks;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using MySystem.Application.Common.Exceptions;
//using MySystem.Application.Common.Interfaces;

//namespace MySystem.Application.Employee.Command.AddEmployeeSession.V1
//{
//    public class AddEmployeeSessionHandler : IRequestHandler<AddEmployeeSessionCommand, IEnumerable<Claim>>
//    {
//        private readonly IMySystemDbContext context;
//        private readonly ICurrentUserInfo currentUser;

//        public AddEmployeeSessionHandler(IMySystemDbContext context, ICurrentUserInfo currentUser)
//        {
//            this.context = context;
//            this.currentUser = currentUser;
//        }

//        public async Task<IEnumerable<Claim>> Handle(AddEmployeeSessionCommand request, CancellationToken cancellationToken)
//        {
//            if (request == null)
//            {
//                throw new ArgumentNullException();
//            }



//            if (request.Payload == null)// || string.IsNullOrWhiteSpace(request.DeviceId) || request.Payload.IsValid() == false)
//            {
//                var claims_ = currentUser.Claims;// jwtService.GetClaims(request.Token);
//                var session = currentUser.SessionId;// claims_.Claims.FirstOrDefault(x => x.Type == UserClaim.Session)?.Value;
//                if (session == null || session == Guid.Empty)
//                {
//                    throw new ArgumentException("Invalid current user details, this should function should not be called if no valid token.");
//                }

//                var employeeid = (await context.EmployeeSessions.FirstOrDefaultAsync(x => x.Id == session && x.TerminationTs == null))?.EmployeeId;
//                var employee = await context.Employees.FirstOrDefaultAsync(x => x.Id == employeeid);
//                if (employee == null)
//                {
//                    throw new NotFoundException("Employee was not found!", "");
//                    //Todo: Log here please
//                }

//                var isAllowedToLogin = await context.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id && x.AccountLockArgument == null);
//                if (isAllowedToLogin == null)
//                {
//                    throw new NotFoundException("Employee was blocked, please contact admin!", "");
//                    //Todo: Log here please
//                }

//                //Todo: Log This Activity

//                var claims = new List<Claim>()
//                {
//                    new Claim(Claims.Types[Claims.ClaimType.Session], session.ToString())
//                };

//                try
//                {
//                    var storedClaims = await context.EmployeeClaims.Where(x => x.EmployeeId == employee.Id && x.RevokedTs == null).ToListAsync(cancellationToken);
//                    storedClaims.ForEach(x => claims.Add(new Claim(x.ClaimType, x.ClaimValue)));
//                }
//                catch (Exception e)
//                { _ = e.Message; }

//                return claims;
//            }
//            else
//            {
//                var employee = await context.Employees.FirstOrDefaultAsync(x => x.Username == request.Payload.Username && x.Password == request.Payload.Password && x.AccountLockArgument == null, cancellationToken);

//                if (employee == null)
//                {
//                    throw new NotFoundException(request.Payload.Username, employee?.Id);
//                }

//                var session = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.Employee == employee && x.TerminationTs == null, cancellationToken);
//                if (session == null)
//                {
//                    session = new();
//                    session.EmployeeId = employee.Id;
//                    session.DeviceId = request.DeviceId;
//                    await context.EmployeeSessions.AddAsync(session, cancellationToken);
//                    await context.SaveChangesAsync(cancellationToken);
//                }

//                var claims = new List<Claim>()
//            {
//                new Claim(Claims.Types[Claims.ClaimType.Session], session.Id.ToString())
//            };

//                return claims;
//            }
//        }
//    }
//}
