using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySystem.Api.Controllers.V1;
using MySystem.Api.Dtos.V1;
using MySystem.Data;
using MySystem.Data.Models.Basic;

namespace MySystem.Api.Controllers.V1
{
    [ApiController]
    [Route("Api/Office")]
    [ApiVersion("1.0")]
    public class OfficeController : BaseController<OfficeDto, OfficeDto, Office>
    {
        public OfficeController(ILogger<OfficeController> logger, DataContext context) : base(logger, new OfficeDto(context))
        {
        }
    }
}
