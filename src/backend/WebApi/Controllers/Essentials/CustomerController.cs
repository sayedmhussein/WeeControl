using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeControl.Application.Contexts.Essential.Commands;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.WebApi.Security.Policies;

namespace WeeControl.WebApi.Controllers.Essentials;

[ApiController]
[Route(Api.Essential.User.Route)]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class CustomerController : UserController
{
    public CustomerController(IMediator mediator) : base(mediator)
    {
    }

    // [AllowAnonymous]
    // [HttpPost]
    // [MapToApiVersion("1.0")]
    // [ProducesResponseType((int)HttpStatusCode.OK)]
    // [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    // [ProducesResponseType((int)HttpStatusCode.Conflict)]
    // public async Task<ActionResult<ResponseDto<AuthenticationResponseDto>>> RegisterV1([FromBody] RequestDto<RegisterCustomerDto> dto)
    // {
    //     var command = new UserRegisterCommand(dto);
    //     var response = await mediator.Send(command);
    //
    //     return Ok(response);
    // }
    
    // [Authorize(Policy = nameof(CanEditUserPolicy))]
    // [HttpGet]
    // [MapToApiVersion("1.0")]
    // public async Task<ActionResult<ResponseDto<IEnumerable<UserDtoV1>>>> GetListOfUsersV1()
    // {
    //     throw new NotImplementedException();
    // }
    //
    // [Authorize]
    // [HttpGet("{username}")]
    // [MapToApiVersion("1.0")]
    // public async Task<ActionResult<ResponseDto<IEnumerable<UserDtoV1>>>> GetUserDetailsV1(string username)
    // {
    //     throw new NotImplementedException();
    //
    //     //return Ok(response);
    // }

    
}