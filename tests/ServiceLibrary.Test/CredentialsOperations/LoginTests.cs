using Xunit;
using Moq;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Frontend.ServiceLibrary.Operations;
using WeeControl.Frontend.ServiceLibrary.Operations.Credentials;

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

        await new CredentialsOperation(null, null).LoginAsync(dto);
        
        
    }
}