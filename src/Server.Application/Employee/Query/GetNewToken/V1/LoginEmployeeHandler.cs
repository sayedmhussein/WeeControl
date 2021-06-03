//using System;
//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Threading;
//using System.Threading.Tasks;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using MySystem.Application.Common.Exceptions;
//using MySystem.Application.Common.Interfaces;
//using MySystem.SharedKernel.Entities.Public.Constants;
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
