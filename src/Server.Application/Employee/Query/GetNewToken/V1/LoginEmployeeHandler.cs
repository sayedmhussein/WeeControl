//using System;
//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Threading;
//using System.Threading.Tasks;
//using Application.Common.Exceptions;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using MySystem.Application.Common.Interfaces;
//using MySystem.SharedKernel.Entites.Public.V1Dto;
//using MySystem.SharedKernel.ExtensionMethods;

//namespace Application.Employee.Query.GetNewToken.V1
//{
//    public class GetNewTokenHandler : IRequestHandler<GetNewTokenQuery, ResponseDto<string>>
//    {
//        private readonly IMySystemDbContext context;
//        private readonly IJwtService jwtService;

//        public GetNewTokenHandler(IMySystemDbContext context, IJwtService jwtService)
//        {
//            this.context = context;
//            this.jwtService = jwtService;
//        }

//        public async Task<ResponseDto<string>> Handle(GetNewTokenQuery request, CancellationToken cancellationToken)
//        {
//            if (request == null)
//            {
//                throw new ArgumentNullException();
//            }

//            if (request.Payload == null || string.IsNullOrWhiteSpace(request.DeviceId) || request.Payload.IsValid() == false)
//            {
//                throw new BadRequestException("Invalid Payload");
//            }

//            var employee = await context.Employees.FirstOrDefaultAsync(x => x.Username == request.Payload.Username && x.Password == request.Payload.Password && x.AccountLockArgument == null, cancellationToken);
//            if (employee == null)
//            {
//                throw new NotFoundException(request.Payload.Username, employee?.Id);
//            }

//            var session = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.Employee == employee && x.TerminationTs == null, cancellationToken);
//            if (session == null)
//            {
//                session = new ();
//                session.EmployeeId = employee.Id;
//                session.DeviceId = request.DeviceId;
//                await context.EmployeeSessions.AddAsync(session, cancellationToken);
//                await context.SaveChangesAsync(cancellationToken);
//            }

//            var claims = new List<Claim>()
//            {
//                new Claim(UserClaim.Session, session.Id.ToString())
//            };
            

//            //try
//            //{
//            //    var storedClaims = await context.EmployeeClaims.Where(x => x.EmployeeId == employee.Id && x.RevokedTs == null).ToListAsync(cancellationToken);
//            //    storedClaims.ForEach(x => claims.Add(new Claim(x.ClaimType, x.ClaimValue)));
//            //}
//            //catch(Exception e)
//            //{ _ = e.Message; }
            
            

            

//            var token = jwtService.GenerateJwtToken(claims, "issuer", DateTime.UtcNow.AddMinutes(5));

//            return new ResponseDto<string>(token);
//        }
//    }
//}
