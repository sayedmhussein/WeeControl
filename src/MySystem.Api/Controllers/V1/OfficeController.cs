using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sayed.MySystem.Api.Controllers.V1;
using Sayed.MySystem.ServerData;
using Sayed.MySystem.ServerData.Models.Basic;
using Sayed.MySystem.SharedDto.V1.Entities;

namespace Sayed.MySystem.Api.Controllers.V1
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
