using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Common.SharedKernel.Obsolute.Employee;
using WeeControl.Common.UserSecurityLib;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Backend.WebApi.Controllers.HumanResources
{
    [Route("Api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IMediator mediatR;
        private readonly IJwtService jwtService;

        public EmployeeController(IMediator mediatR, IJwtService jwtService)
        {
            this.mediatR = mediatR;
            this.jwtService = jwtService;
        }
        
        #region Employee
        [HttpGet]
        [MapToApiVersion("1.0")]
        public Task<IEnumerable<EmployeeDto>> GetEmployeesV1(Guid? territoryid, Guid? employeeid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Create New Employee Or Update Current Employee Within the Organization
        /// </summary>
        /// 
        /// <remarks>
        /// Authorized User:
        ///
        ///     1. Must provide brear token issued 15 minutes ago or less.
        ///     2. Only Human Resources Employee who has claim tag "add" and working in office which belong to the added employee or higher.
        ///
        /// </remarks>
        /// 
        /// <param name="command">
        ///     Employee DTO as payload inside Request DTO
        /// </param>
        /// 
        /// <returns>
        ///     The newely created Employee DTO
        /// </returns>
        /// 
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">DTO has missing/invalid values</response>  
        [Authorize(SecurityPolicies.Employee.CanAlterEmployee)]
        [HttpPut]
        //[Consumes(MediaTypeNames.Application.Json)]
        //[Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [MapToApiVersion("1.0")]
        // public async Task<ActionResult<IEnumerable<EmployeeDto>>> PutEmployeeV1([FromBody] AddEmployeeCommand command)
        // {
        //     var response = await mediatR.Send(command);
        //     return Created("Api/[controller]/", response);
        // }

        [Authorize(SecurityPolicies.Employee.CanAlterEmployee)]
        [HttpDelete("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [MapToApiVersion("1.0")]
        public Task<ActionResult<EmployeeDto>> DeleteEmployeeV1(Guid id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Employee Claim
        [HttpGet("Claim/{employeeid}")]
        public async Task<ActionResult<IEnumerable<EmployeeClaimDto>>> GetEmployeeClaimsV1(Guid employeeid)
        {
            // var response = await mediatR.Send(new GetEmployeeClaimsQuery() { EmployeeId = employeeid });
            // var claims = new List<EmployeeClaimDto>();
            // foreach (var claim in response)
            // {
            //     claims.Add(new EmployeeClaimDto() { });
            // }
            //
            // return Ok(new List<EmployeeClaimDto>(claims));
            throw new NotImplementedException();
        }

        [HttpPut("Claim")]
        public Task<ActionResult> PutEmployeeClaimV1()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("Claim/{claimid}")]
        public Task<ActionResult> PutEmployeeClaimV1(Guid claimid)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Employee Indentity
        [HttpGet("Identity/{employeeid}")]
        public Task<ActionResult> GetEmployeeIdentitiesV1(Guid employeeid)
        {
            throw new NotImplementedException();
        }

        [HttpGet("Identity/{employeeid}/{attachmentid}")]
        public async Task<ActionResult> DownloadEmployeeIdentityAttachmentV1(Guid employeeid, Guid attachmentid)
        {
            Stream stream = null;// await { { __get_stream_based_on_id_here__} }
            await Task.Delay(1);

            if (stream == null)
                return NotFound(); // returns a NotFoundResult with Status404NotFound response.

            return File(stream, "application/octet-stream"); // returns a FileStreamResult
        }

        [HttpPut("Identity/{employeeid}")]
        public Task<ActionResult> PutEmployeeIdentity(Guid employeeid)
        {
            throw new NotImplementedException();
        }

        [HttpPut("Identity/{employeeid}/Attachment")]
        public async Task<ActionResult> PutEmployeeIdentity(Guid employeeid, List<IFormFile> files)
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
                        await Task.Delay(1);
                        //var filePath = Path.Combine(config["StoredFilesPath"], "EmployeePhotos",
                        //    Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName).ToLowerInvariant());

                        //path = filePath;

                        //using var stream = System.IO.File.Create(filePath);
                        //await formFile.CopyToAsync(stream);
                    }
                }

                // Process uploaded files
                // Don't rely on or trust the FileName property without validation.

                return Ok(new { count = files.Count, size, path });
            }

            return BadRequest();
        }

        [HttpDelete("Identity/{employeeid}/{identityid}")]
        public Task<ActionResult> DeleteEmployeeIdentityV1(Guid employeeid, Guid identityid)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("Identity/{employeeid}/Attachment/{attachmentid}")]
        public Task<ActionResult> DeleteEmployeeIdentityAttachmentV1(Guid employeeid, Guid attachmentid)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
