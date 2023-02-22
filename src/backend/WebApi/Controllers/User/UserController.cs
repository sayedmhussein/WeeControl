 using System;
 using System.Net;
 using System.Net.Mime;
 using System.Threading.Tasks;
 using MediatR;
 using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Mvc;
 using WeeControl.Core.Application.Contexts.User.Commands;
 using WeeControl.Core.Application.Contexts.User.Queries;
 using WeeControl.Core.DataTransferObject.BodyObjects;
 using WeeControl.Core.DataTransferObject.Contexts.User;
 using WeeControl.Core.SharedKernel;
 using WeeControl.Host.WebApiService;

 namespace WeeControl.Host.WebApi.Controllers.User;
 
 [ApiController]
 [Authorize]
 [Route(ControllerApi.User.Route)]
 [Consumes(MediaTypeNames.Application.Json)]
 [Produces(MediaTypeNames.Application.Json)]
 public class UserController : Controller
 {
     private readonly IMediator mediator;

     public UserController(IMediator mediator)
     {
         this.mediator = mediator;
     }

     #region Endpoint

     [AllowAnonymous]
     [HttpPost]
     [MapToApiVersion("1.0")]
     [ProducesResponseType((int)HttpStatusCode.OK)]
     [ProducesResponseType((int)HttpStatusCode.BadRequest)]
     [ProducesResponseType((int)HttpStatusCode.Conflict)]
     public async Task<ActionResult<ResponseDto<TokenResponseDto>>> RegisterV1([FromBody] RequestDto<CustomerRegisterDto> dto)
     {
         var command = new UserRegisterCommand(dto);
         var response = await mediator.Send(command);

         return Ok(response);
     }

     [HttpGet]
     [MapToApiVersion("1.0")]
     [ProducesResponseType((int) HttpStatusCode.OK)]
     public async Task<ActionResult<ResponseDto<HomeResponseDto>>> GetHomeV1()
     {
         var query = new HomeQuery();
         var response = await mediator.Send(query);

         return Ok(response);
     }

     #endregion
     
     [Authorize]
     [HttpPatch(ControllerApi.User.PasswordEndpoint)]
     [MapToApiVersion("1.0")]
     [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
     [ProducesResponseType((int)HttpStatusCode.NotFound)]
     [ProducesResponseType((int)HttpStatusCode.BadRequest)]
     [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
     public async Task<ActionResult> SetNewPasswordV1([FromBody] RequestDto<UserPasswordChangeRequestDto> dto)
     {
         var command = new UserNewPasswordCommand(dto, dto.Payload.OldPassword, dto.Payload.NewPassword);
         var response = await mediator.Send(command);

         return Ok();
     }

     [AllowAnonymous]
     [HttpPost(ControllerApi.User.PasswordEndpoint)]
     [MapToApiVersion("1.0")]
     [ProducesResponseType((int)HttpStatusCode.OK)]
     [ProducesResponseType((int)HttpStatusCode.BadRequest)]
     public async Task<ActionResult> ResetPasswordV1([FromBody] RequestDto<UserPasswordResetRequestDto> dto)
     {
         var command = new UserForgotMyPasswordCommand(dto);
         await mediator.Send(command);

         return Ok();
     }

     [HttpDelete(ControllerApi.User.NotificationEndpoint + "/{id:guid}")]
     [MapToApiVersion("1.0")]
     public async Task<ActionResult> MuteNotificationV1(Guid id)
     {
         var command = new NotificationViewedCommand(id);
         await mediator.Send(command);
         return Ok();
     }



 }