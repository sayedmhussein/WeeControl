//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using MediatR;
//using MySystem.Application.Common.Exceptions;
//using MySystem.Application.Common.Interfaces;
//using MySystem.SharedKernel.Entites.Public.V1Dto;
//using MySystem.SharedKernel.Entities.Employee.V1Dto;

//namespace WeeControl.Server.Application.Aggregates.Employee.Commands.AssignClaimV1
//{
//    public class AddEmployeeClaimHandler : IRequestHandler<AddEmployeeClaimCommand, ResponseDto<EmployeeClaimDto>>
//    {
//        private readonly IMySystemDbContext context;
//        private readonly ICurrentUserInfo currentUser;

//        public AddEmployeeClaimHandler(IMySystemDbContext context, ICurrentUserInfo currentUser)
//        {
//            this.context = context;
//            this.currentUser = currentUser;
//        }

//        public Task<ResponseDto<EmployeeClaimDto>> Handle(AddEmployeeClaimCommand request, CancellationToken cancellationToken)
//        {
//            var admin_ = currentUser.Claims.FirstOrDefault(x => x.Type == EmployeePosition.Temporary.ToString())?.Value;
//            if (admin_ != null && admin_.Contains("add"))
//            { }
//            else
//            {
//                throw new NotAllowedException("");
//            }

//            throw new NotImplementedException();
//        }
//    }
//}
