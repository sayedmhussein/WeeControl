using System.Collections;
using System.Collections.Generic;
using System.Net;
using MySystem.SharedKernel.Entities.Employee.V1Dto;
using MySystem.SharedKernel.Entities.Public.V1Dto;

namespace MySystem.Web.Api.Test.Controller.V1.Employee.Credentials
{
    public class LoginTestsClassData : IEnumerable<object[]>
    {
        public LoginTestsClassData()
        {
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, HttpStatusCode.BadRequest, false };
            yield return new object[] { new object(), HttpStatusCode.BadRequest, false };
            yield return new object[] { new ResponseDto<bool>(), HttpStatusCode.BadRequest, false };
            yield return new object[] { new RequestDto<bool>(), HttpStatusCode.BadRequest, false };
            yield return new object[] { new RequestDto<object>(), HttpStatusCode.BadRequest, false };
            yield return new object[] { new RequestDto<LoginDto>(), HttpStatusCode.BadRequest, false };

            yield return new object[] { new RequestDto<LoginDto>() { DeviceId = string.Empty }, HttpStatusCode.BadRequest, false };
            yield return new object[] { new RequestDto<LoginDto>() { DeviceId = "Device", Payload = null }, HttpStatusCode.BadRequest, false };

            yield return new object[] { new RequestDto<LoginDto>() { DeviceId = "Device", Payload = new LoginDto() { Username = null, Password = "password" } }, HttpStatusCode.BadRequest, false };
            yield return new object[] { new RequestDto<LoginDto>() { DeviceId = "Device", Payload = new LoginDto() { Username = string.Empty, Password = string.Empty } }, HttpStatusCode.BadRequest, false };
            yield return new object[] { new RequestDto<LoginDto>() { DeviceId = "Device", Payload = new LoginDto() { Username = "invalid", Password = string.Empty } }, HttpStatusCode.BadRequest, false };
            yield return new object[] { new RequestDto<LoginDto>() { DeviceId = "Device", Payload = new LoginDto() { Username = string.Empty, Password = "password" } }, HttpStatusCode.BadRequest, false };
            yield return new object[] { new RequestDto<LoginDto>() { DeviceId = "Device", Payload = new LoginDto() { Username = "invalid", Password = null } }, HttpStatusCode.BadRequest, false };

            yield return new object[] { new RequestDto<LoginDto>() { DeviceId = "Device", Payload = new LoginDto() { Username = "invalid", Password = "password" } }, HttpStatusCode.NotFound, false };
            yield return new object[] { new RequestDto<LoginDto>() { DeviceId = "Device", Payload = new LoginDto() { Username = "username", Password = "password" } }, HttpStatusCode.OK, true };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
