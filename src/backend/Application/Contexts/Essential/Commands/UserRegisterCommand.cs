// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using MediatR;
// using WeeControl.Application.Contexts.Essential.Queries;
// using WeeControl.Application.Exceptions;
// using WeeControl.Application.Interfaces;
// using WeeControl.Domain.Contexts.Essential;
// using WeeControl.SharedKernel.Essential.DataTransferObjects;
// using WeeControl.SharedKernel.Essential.Entities;
// using WeeControl.SharedKernel.Interfaces;
// using WeeControl.SharedKernel.RequestsResponses;
//
// namespace WeeControl.Application.Contexts.Essential.Commands;
//
// public class UserRegisterCommand : IRequest<IResponseDto<AuthenticationResponseDto>>
// {
//     private readonly IRequestDto request;
//     private readonly PersonalEntity person;
//     private readonly UserEntity user;
//     private readonly EmployeeEntity employee;
//     private readonly CustomerEntity customer;
//
//     public UserRegisterCommand(IRequestDto<RegisterCustomerDto> dto)
//     {
//         request = dto;
//         person = dto.Payload.Personal;
//         user = dto.Payload.User;
//         customer = dto.Payload.Customer;
//     }
//
//     public UserRegisterCommand(IRequestDto<RegisterEmployeeDto> dto)
//     {
//         request = dto;
//         person = dto.Payload.Personal;
//         user = dto.Payload.User;
//         employee = dto.Payload.Employee;
//     }
//     
//     public class RegisterHandler : IRequestHandler<UserRegisterCommand, IResponseDto<AuthenticationResponseDto>>
//     {
//         private readonly IEssentialDbContext context;
//         private readonly IMediator mediator;
//         private readonly IPasswordSecurity passwordSecurity;
//
//         public RegisterHandler(IEssentialDbContext context, IMediator mediator, IPasswordSecurity passwordSecurity)
//         {
//             this.context = context;
//             this.mediator = mediator;
//             this.passwordSecurity = passwordSecurity;
//         }
//
//         public async Task<IResponseDto<AuthenticationResponseDto>> Handle(UserRegisterCommand cmd, CancellationToken cancellationToken)
//         {
//             //await mediator.Send(new VerifyRequestQuery(cmd.dto), cancellationToken);
//
//             if (string.IsNullOrWhiteSpace(cmd.person.FirstName) ||
//                 string.IsNullOrWhiteSpace(cmd.person.LastName) ||
//                 string.IsNullOrWhiteSpace(cmd.user.Email) ||
//                 string.IsNullOrWhiteSpace(cmd.user.Username) ||
//                 string.IsNullOrWhiteSpace(cmd.user.Password) ||
//                 string.IsNullOrWhiteSpace(cmd.user.MobileNo)
//                )
//             {
//                 throw new ValidationException();
//             }
//
//             if (context.Users.Any(x => 
//                     x.Username == cmd.user.Username.ToLower() ||
//                     x.Email == cmd.user.Email.ToLower() ||
//                     x.MobileNo == cmd.user.MobileNo.ToLower()
//                     ))
//             {
//                 throw new ConflictFailureException();
//             }
//
//             cmd.user.Email = cmd.user.Email.ToLower();
//             cmd.user.Username = cmd.user.Username.ToLower();
//             cmd.user.MobileNo = cmd.user.MobileNo.ToLower();
//
//             var user = new UserDbo(cmd.user)
//             {
//                 Password = passwordSecurity.Hash(cmd.user.Password)
//             };
//
//             await context.Users.AddAsync(user, cancellationToken);
//             await context.SaveChangesAsync(cancellationToken);
//
//             var request =
//                 new UserTokenQuery(RequestDto.Create(LoginDtoV1.Create(user.Username, cmd.user.Password),
//                     cmd.request));
//             var response = await mediator.Send(request, cancellationToken);
//             return response;
//         }
//     }
// }