//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading;
//using System.Threading.Tasks;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using MySystem.Application.Common.Interfaces;
//using MySystem.SharedKernel.Definition;
//using MySystem.SharedKernel.Dto.V1;
//using MySystem.SharedKernel.Entites.Public.V1Dto;

//namespace Application.Employee.Command.GetRefreshedToken.V1
//{
//    public class GetRefreshedTokenHandler : IRequestHandler<GetRefreshedTokenQuery, ResponseDto<string>>
//    {
//        private readonly IMySystemDbContext context;
//        private readonly ICurrentUserInfo userInfo;
//        private readonly IJwtService jwtService;

//        public GetRefreshedTokenHandler(IMySystemDbContext context, ICurrentUserInfo userInfo, IJwtService jwtService)
//        {
//            this.context = context;
//            this.userInfo = userInfo;
//            this.jwtService = jwtService;
//        }

//        public async Task<ResponseDto<string>> Handle(GetRefreshedTokenQuery request, CancellationToken cancellationToken)
//        {
//            var claims_ = userInfo.Claims;// jwtService.GetClaims(request.Token);
//            var session = userInfo.SessionId;// claims_.Claims.FirstOrDefault(x => x.Type == UserClaim.Session)?.Value;
//            if (session == Guid.Empty)
//            {
//                return new ResponseDto<string>(string.Empty);
//            }

//            var employee = await context.EmployeeSessions.FirstOrDefaultAsync(x => x.Id == session && x.TerminationTs == null);
//            if (employee == null)
//            {
//                return new ResponseDto<string>(string.Empty);
//                //Todo: Log here please
//            }

//            var isAllowedToLogin = await context.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id && x.AccountLockArgument == null);
//            if (isAllowedToLogin != null)
//            {
//                return new ResponseDto<string>(string.Empty);
//                //Todo: Log here please
//            }

//            //Todo: Log This Activity

//            var claims = new List<Claim>()
//            {
//                new Claim(UserClaim.Session, session.ToString())
//            };


//            try
//            {
//                var storedClaims = await context.EmployeeClaims.Where(x => x.EmployeeId == employee.Id && x.RevokedTs == null).ToListAsync(cancellationToken);
//                storedClaims.ForEach(x => claims.Add(new Claim(x.ClaimType, x.ClaimValue)));
//            }
//            catch (Exception e)
//            { _ = e.Message; }

//            var token = jwtService.GenerateJwtToken(claims, "issuer", DateTime.UtcNow.AddDays(5));

//            return new ResponseDto<string>(token);
//        }
//    }
//}
