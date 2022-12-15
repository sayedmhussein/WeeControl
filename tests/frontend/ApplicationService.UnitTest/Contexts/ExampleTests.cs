using System;
using System.Net;
using WeeControl.Common.SharedKernel.DataTransferObjects.User;
using WeeControl.Common.SharedKernel.RequestsResponses;
using WeeControl.Frontend.AppService;

namespace WeeControl.Frontend.Service.UnitTest.Contexts;

public class ExampleTests
{
    #region Function1()
    [Theory]
    [InlineData(HttpStatusCode.OK)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public void Function1_ExpectedResponsesBehaviorTests(HttpStatusCode code)
    {
        using var helper = new TestHelper(nameof(Function1_ExpectedResponsesBehaviorTests));
        var content = helper.GetJsonContent(ResponseDto.Create(AuthenticationResponseDto.Create("token", "name")));
        var device = helper.DeviceMock.GetObject(code, content);
        
        //Service Action...
        
        switch (code)
        {
            case HttpStatusCode.OK:
                helper.DeviceMock.AlertMock.Verify(x => 
                    x.DisplayAlert(It.IsAny<string>()), Times.Never);
                helper.DeviceMock.NavigationMock.Verify(x => 
                    x.NavigateToAsync(ApplicationPages.Essential.SplashPage,It.IsAny<bool>()), Times.Never);
                
                helper.DeviceMock.SecurityMock.Verify(x => 
                    x.UpdateTokenAsync("token"), Times.Never);
                break;
            case HttpStatusCode.NotFound:
                break;
            case HttpStatusCode.InternalServerError:
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.BadGateway:
                helper.DeviceMock.AlertMock.Verify(x => 
                    x.DisplayAlert(It.IsAny<string>()), Times.Never);
                helper.DeviceMock.NavigationMock.Verify(x => 
                    x.NavigateToAsync(ApplicationPages.Essential.HomePage,true), Times.Never);
                
                helper.DeviceMock.SecurityMock.Verify(x => 
                    x.UpdateTokenAsync("token"), Times.Never);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(code), code, null);
        }
    }

    [Theory]
    [InlineData("0")]
    public void Function1_DataTransferObjectDefensiveValues(string value)
    {
        using var helper = new TestHelper(nameof(Function1_DataTransferObjectDefensiveValues));
        
        // Service Action
        
        helper.DeviceMock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Never);
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(ApplicationPages.Essential.SplashPage,true), Times.Never);
                
        helper.DeviceMock.SecurityMock.Verify(x => 
            x.UpdateTokenAsync("token"), Times.Never);
    }
    #endregion
}