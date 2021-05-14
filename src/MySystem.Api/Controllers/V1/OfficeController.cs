using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sayed.MySystem.Api.Controllers.V1;
using Sayed.MySystem.EntityFramework;
using Sayed.MySystem.EntityFramework.Models.Basic;
using Sayed.MySystem.Shared.Dbos;
using Sayed.MySystem.Shared.Dtos.V1.Entities;

namespace Sayed.MySystem.Api.Controllers.V1
{
    [ApiController]
    [Route("Api/Office")]
    [ApiVersion("1.0")]
    public class OfficeController : BaseController<OfficeDto, OfficeDbo>
    {
        public OfficeController(ILogger<OfficeController> logger, DataContext context) : base(logger, context)
        {
        }
    }
}
