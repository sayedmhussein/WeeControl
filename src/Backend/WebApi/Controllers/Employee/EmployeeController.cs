using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Backend.WebApi.Controllers.Employee
{
    [Route("Api/[controller]")]
    [ApiController]
    public partial class EmployeeController : Controller
    {
        private readonly IMediator mediatR;
        private readonly IJwtService jwtService;

        public EmployeeController(IMediator mediatR, IJwtService jwtService)
        {
            this.mediatR = mediatR;
            this.jwtService = jwtService;
        }
    }
}
