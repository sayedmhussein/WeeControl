using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.Domain.Contexts.Essentials;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.SharedKernel;
using WeeControl.Core.SharedKernel.Contexts.Essentials;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.Application.Contexts.Essentials.Commands;

public class UserRegisterCommand : IRequest<ResponseDto<TokenResponseDto>>
{
    private readonly RequestDto request;
    private readonly PersonModel person;
    private readonly UserModel user;
    private readonly EmployeeModel employee;
    private readonly CustomerModel customer;

    public UserRegisterCommand(RequestDto<CustomerRegisterDto> dto) : this(dto, dto.Payload.Person, dto.Payload.User)
    {
        dto.Payload.Customer.ThrowExceptionIfEntityModelNotValid();
        customer = dto.Payload.Customer;
    }

    public UserRegisterCommand(RequestDto<EmployeeRegisterDto> dto) : this(dto, dto.Payload.Person, dto.Payload.User)
    {
        dto.Payload.Employee.ThrowExceptionIfEntityModelNotValid();
        employee = dto.Payload.Employee;
    }

    private UserRegisterCommand()
    {
    }

    private UserRegisterCommand(RequestDto requestDto, PersonModel person, UserModel user)
    {
        requestDto.ThrowExceptionIfEntityModelNotValid();
        request = requestDto;
        
        person.ThrowExceptionIfEntityModelNotValid();
        this.person = person;

        user.ThrowExceptionIfEntityModelNotValid();
        this.user = user;
    }

    public class RegisterHandler : IRequestHandler<UserRegisterCommand, ResponseDto<TokenResponseDto>>
    {
        private readonly IEssentialDbContext context;
        private readonly IMediator mediator;
        private readonly IPasswordSecurity passwordSecurity;

        public RegisterHandler(IEssentialDbContext context, IMediator mediator, IPasswordSecurity passwordSecurity)
        {
            this.context = context;
            this.mediator = mediator;
            this.passwordSecurity = passwordSecurity;
        }

        public async Task<ResponseDto<TokenResponseDto>> Handle(UserRegisterCommand cmd, CancellationToken cancellationToken)
        {
            if (context.Users.Any(x =>
                    x.Username == cmd.user.Username.ToLower() ||
                    x.Email == cmd.user.Email.ToLower() ||
                    x.MobileNo == cmd.user.MobileNo.ToLower()
                ))
            {
                throw new ConflictFailureException();
            }

            var person = PersonDbo.Create(cmd.person);
            await context.Person.AddAsync(person, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var user = UserDbo.Create(person.PersonId, cmd.user.Email, cmd.user.Username, cmd.user.MobileNo, passwordSecurity.Hash(cmd.user.Password));
            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            if (cmd.customer is not null)
            {
                await context.Customers.AddAsync(CustomerDbo.Create(user.UserId, cmd.customer), cancellationToken);
            }
            else
            {
                await context.Employees.AddAsync(EmployeeDbo.Create(person.PersonId, null, cmd.employee), cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);

            var request =
                new SessionCreateCommand(RequestDto.Create(
                    LoginRequestDto.Create(user.Username, cmd.user.Password),
                    cmd.request));
            var response = await mediator.Send(request, cancellationToken);
            return response;
        }
    }
}