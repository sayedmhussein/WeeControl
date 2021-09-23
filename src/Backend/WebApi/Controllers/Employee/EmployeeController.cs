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
        private readonly IJwtServiceObsolute jwtServiceObsolute;

        public EmployeeController(IMediator mediatR, IJwtServiceObsolute jwtServiceObsolute)
        {
            this.mediatR = mediatR;
            this.jwtServiceObsolute = jwtServiceObsolute;
        }
    }
}
