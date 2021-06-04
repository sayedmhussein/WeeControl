//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading;
//using System.Threading.Tasks;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using MySystem.Application.Common.Interfaces;
//using MySystem.SharedKernel.Entities.Public.V1Dto;

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
            

//            var token = jwtService.GenerateJwtToken(claims, "issuer", DateTime.UtcNow.AddDays(5));

//            return new ResponseDto<string>(token);
//        }
//    }
//}
