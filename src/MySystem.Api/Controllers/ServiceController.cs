using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySystem.Api.Dtos;
using MySystem.Data.Data;
using MySystem.Data.Models.Basic;
using MySystem.Data.Models.Component;

namespace MySystem.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class ServiceController : Controller
    {
        private readonly DataContext db;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(DataContext db, ILogger<ServiceController> logger)
        {
            this.db = db;
            _logger = logger;
        }

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IEnumerable<PortfolioV1Dto>>> GetPortfolioV1()
        {
            var query = from building in db.Buildings
                        join unit in db.Units on building.BuildingId equals unit.BuildingId
                        join cu in db.ContractUnits on unit.UnitId equals cu.UnitId
                        join contract in db.Contracts on cu.ContractId equals contract.ContractId
                        where cu.CancellationTs == null
                        select new { building, unit, contract };

            var result = new List<PortfolioV1Dto>();

            foreach (var item in query.ToList())
            {
                result.Add(new PortfolioV1Dto()
                {
                    ContractId = item.contract.ContractId,
                    BuildingId = item.building.BuildingId,
                    UnitId = item.unit.UnitId,

                    ContractNo = item.contract.ContractNo,
                    UnitNo = item.unit.UnitNo,
                    UnitType = (int)item.unit.UnitType,

                    BuildingName = item.building.BuildingName,
                    LastVisitedBy = null,
                    LastVisitedTs = null,
                    NextVisitTs = null,

                    ContractName = item.contract.ContractName,
                    UnitStatues = new List<int>()
                }) ;
            }

            await Task.Delay(1);

            return Ok(result);
        }
    }
}
