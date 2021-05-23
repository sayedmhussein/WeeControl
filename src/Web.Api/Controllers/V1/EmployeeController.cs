using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySystem.Web.EntityFramework;
using MySystem.Web.EntityFramework.Models.People;
using MySystem.Web.Shared.Dbos;
using MySystem.Web.Shared.Dtos.V1.Entities;

namespace MySystem.Web.Api.Controllers.V1
{
    [ApiController]
    [Route("Api/[controller]")]
    [ApiVersion("1.0")]
    public class EmployeeController : BaseController<EmployeeDto, EmployeeDbo>
    {
        private readonly ILogger<EmployeeController> logger;
        private readonly DataContext context;
        private readonly IConfiguration config;

        public EmployeeController(ILogger<EmployeeController> logger, DataContext context, IConfiguration config ) : base(logger, context)
        {
            this.logger = logger;
            this.context = context;
            this.config = config;
        }

        [HttpPost("photo")]
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            string[] permittedExtensions = { ".jpg", ".png", ".pdf" };

            if (files.TrueForAll(x => permittedExtensions.Contains(Path.GetExtension(x.FileName).ToLowerInvariant())))
            {
                long size = files.Sum(f => f.Length);
                string path = String.Empty;

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = Path.Combine(config["StoredFilesPath"], "EmployeePhotos",
                            Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName).ToLowerInvariant());
                        //Directory.CreateDirectory(filePath);
                        path = filePath;

                        using var stream = System.IO.File.Create(filePath);
                        await formFile.CopyToAsync(stream);
                    }
                }

                // Process uploaded files
                // Don't rely on or trust the FileName property without validation.

                return Ok(new { count = files.Count, size, path });
            }

            return BadRequest();
        }
    }
}
