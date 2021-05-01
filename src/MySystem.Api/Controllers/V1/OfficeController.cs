using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySystem.Api.Controllers.V1;
using MySystem.ServerData;
using MySystem.ServerData.Models.Basic;
using MySystem.SharedDto.V1.Entities;

namespace MySystem.Api.Controllers.V1
{
    [ApiController]
    [Route("Api/Office")]
    [ApiVersion("1.0")]
    public class OfficeController : BaseController<OfficeDto, Office>
    {
        public OfficeController(ILogger<OfficeController> logger, DataContext context) : base(logger, context)
        {
        }
    }
}
