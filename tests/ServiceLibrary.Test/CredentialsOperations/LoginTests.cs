using Xunit;
using Moq;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User;
using WeeControl.Frontend.ServiceLibrary.BoundedContexts.Authorization;
using WeeControl.Frontend.ServiceLibrary.Interfaces;

namespace WeeControl.Common.ServiceLibrary.Test.CredentialsOperations;

public class LoginTests
{
    private readonly Mock<IUserDevice> userDeviceMock;
    
    public LoginTests()
    {
        userDeviceMock = new Mock<IUserDevice>();
        userDeviceMock.SetupAllProperties();
        userDeviceMock.Setup(x => x.DeviceId).Returns(nameof(LoginTests));
    }

    [Fact]
    public async void WhenValidLoginCredentials_TokenIsSaved()
    {
        var dto = new LoginDto() { UsernameOrEmail = "admin", Password = "admin"};

        await new UserOperation(null, null).LoginAsync(dto);
        
        
    }
}