using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeeControl.ApiApp.WebApi.Controllers.User;
using WeeControl.Common.SharedKernel;

namespace WeeControl.ApiApp.WebApi.Controllers.Essentials;

[ApiController]
[Route(Api.Essential.Employee.EmployeeRoute)]
public class EmployeeController : UserController
{
    public EmployeeController(IMediator mediator) : base(mediator)
    {
    }
}