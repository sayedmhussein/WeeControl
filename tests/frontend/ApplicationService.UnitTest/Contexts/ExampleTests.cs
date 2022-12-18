using System;
using System.Net;
using WeeControl.Common.SharedKernel.Contexts.Authentication;
using WeeControl.Frontend.AppService;
using WeeControl.Frontend.AppService.GuiInterfaces.Authorization;

namespace WeeControl.Frontend.Service.UnitTest.Contexts;

public class ExampleTests
{
    [Theory]
    [InlineData(HttpStatusCode.OK)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public void Function1_ExpectedResponsesBehaviorTests(HttpStatusCode code)
    {
        using var helper = new TestHelper(nameof(Function1_ExpectedResponsesBehaviorTests));
        var service = helper.GetService<IAuthorizationService>(code, TokenResponseDto.Create("token", "name"));

        service.Login("username", "password");
        
        switch (code)
        {
            case HttpStatusCode.OK:
                // helper.DeviceMock.Verify(x => 
                //     x.DisplayAlert(It.IsAny<string>()), Times.Never);
                helper.DeviceMock.Verify(x => 
                    x.NavigateToAsync(ApplicationPages.Essential.SplashPage,It.IsAny<bool>()), Times.Never);
                
                // helper.DeviceMock.Verify(x => 
                //     x.UpdateTokenAsync("token"), Times.Never);
                break;
            case HttpStatusCode.NotFound:
                break;
            case HttpStatusCode.InternalServerError:
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.BadGateway:
                helper.DeviceMock.Verify(x => 
                    x.NavigateToAsync(ApplicationPages.Essential.HomePage,true), Times.Never);
                
                // helper.DeviceMock.Verify(x => 
                //     x.UpdateTokenAsync("token"), Times.Never);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(code), code, null);
        }
    }
}