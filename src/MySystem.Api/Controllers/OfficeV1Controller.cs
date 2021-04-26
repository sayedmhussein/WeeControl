using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySystem.Data.Data;
using MySystem.Data.Models.Basic;
using MySystem.Data.V1.Dtos;

namespace MySystem.Api.Controllers
{
    [ApiController]
    [Route("Api/V1/Office")]
    public class OfficeV1Controller : BaseV1Controller<OfficeV1Dto, OfficeV1Dto, Office>
    {
        public OfficeV1Controller(ILogger<OfficeV1Controller> logger, DataContext context) : base(logger, new OfficeV1Dto(context))
        {
        }
    }
}
