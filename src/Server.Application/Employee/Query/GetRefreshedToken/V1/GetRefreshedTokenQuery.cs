using System.Collections.Generic;
using System.Security.Claims;
using MediatR;

namespace Application.Employee.Command.GetRefreshedToken.V1
{
    public class GetRefreshedTokenQuery : IRequest<IEnumerable<Claim>>
    {
    }
}
