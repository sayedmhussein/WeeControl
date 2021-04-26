using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySystem.Data.Data;
using MySystem.Data.Models.People;
using MySystem.Data.V1.Dtos;

namespace MySystem.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class EmployeeController : BaseV1Controller<EmployeeV1Dto, EmployeeV1Dto, Employee>
    {
        private readonly ILogger<EmployeeController> logger;
        private readonly DataContext context;

        public EmployeeController(ILogger<EmployeeController> logger, DataContext context) : base(logger, new EmployeeV1Dto(context))
        {
            this.logger = logger;
            this.context = context;
        }
    }
}
