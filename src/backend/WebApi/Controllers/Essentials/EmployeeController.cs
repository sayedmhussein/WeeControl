using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeeControl.SharedKernel;

namespace WeeControl.WebApi.Controllers.Essentials;

[ApiController]
[Route(Api.Essential.Routes.Employee)]
public class EmployeeController : UserController
{
    public EmployeeController(IMediator mediator) : base(mediator)
    {
    }
}