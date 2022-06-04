using System.Threading;
using Microsoft.EntityFrameworkCore;
using Moq;
using WeeControl.Application.Essential.Commands;
using WeeControl.Application.Essential.Queries;
using WeeControl.Application.Exceptions;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using Xunit;

namespace WeeControl.Application.Test.Essential.Commands;

public class RegisterCommandTests
{
    [Fact]
    public async void WhenRegisterNewUser_ReturnSuccessAndToken()
    {
        using var testHelper = new TestHelper();
        testHelper.MediatorMock.Setup(x => x.Send(It.IsAny<GetNewTokenQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ResponseDto<TokenDtoV1>( TokenDtoV1.Create("token", string.Empty, string.Empty)));
        var command = new RegisterCommand(RequestDto.Create<RegisterDtoV1>(RegisterDtoV1.Create("email@emial.com", "username", "password"), "device", null, null));
            
        var tokenDto = await new RegisterCommand.RegisterHandler(testHelper.EssentialDb, testHelper.MediatorMock.Object, testHelper.PasswordSecurity).Handle(command, default);

        testHelper.MediatorMock.Verify(mock => mock.Send(It.IsAny<VerifyRequestQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotEmpty(tokenDto.Payload.Token);
    }
    
    [Fact]
    public async void WhenRegisterNewUserWithCapital_DataInDbAreSmallLetters()
    {
        using var testHelper = new TestHelper();
        string email = "Email@someprovider.com";
        string username = "ThisIsUsername";
        testHelper.MediatorMock.Setup(x => x.Send(It.IsAny<GetNewTokenQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ResponseDto<TokenDtoV1>(TokenDtoV1.Create("token", string.Empty, string.Empty)));
        var command = new RegisterCommand(RequestDto.Create(RegisterDtoV1.Create(email.ToUpper(), username.ToUpper(),  "password"),"device", null, null));
            
        await new RegisterCommand.RegisterHandler(testHelper.EssentialDb, testHelper.MediatorMock.Object, testHelper.PasswordSecurity).Handle(command, default);

        var byEmail = await testHelper.EssentialDb.Users.FirstOrDefaultAsync(x => x.Email == email.ToLower());
        Assert.NotNull(byEmail);
        
        var byUsername = await testHelper.EssentialDb.Users.FirstOrDefaultAsync(x => x.Username == username.ToLower());
        Assert.NotNull(byUsername);
    }
    
    [Fact]
    public async void WhenRegisterNewUser_PasswordIsEncrypted()
    {
        using var testHelper = new TestHelper();
        string postedPassword = "ThisIsPostedPassword";
        testHelper.MediatorMock.Setup(x => x.Send(It.IsAny<GetNewTokenQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ResponseDto<TokenDtoV1>(TokenDtoV1.Create("token", string.Empty, string.Empty)));
        var command = new RegisterCommand(RequestDto.Create(RegisterDtoV1.Create("email@emial.com", "username", "password"),"device",  null, null));
            
        await new RegisterCommand.RegisterHandler(testHelper.EssentialDb, testHelper.MediatorMock.Object, testHelper.PasswordSecurity).Handle(command, default);

        var savedPassword = await testHelper.EssentialDb.Users.FirstOrDefaultAsync(x => x.Username == "username");

        Assert.NotEqual(postedPassword, savedPassword.Password);
    }
    
    [Fact]
    public async void WhenRegisterNewUser_VerifyingCorrectDeviceIsExecuted()
    {
        using var testHelper = new TestHelper();
        testHelper.MediatorMock.Setup(x => x.Send(It.IsAny<GetNewTokenQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ResponseDto<TokenDtoV1>(TokenDtoV1.Create("token", string.Empty, string.Empty)));
        var command = new RegisterCommand (RequestDto.Create(RegisterDtoV1.Create("email@emial.com", "username", "password"),"device",  null, null));
            
        var tokenDto = await new RegisterCommand.RegisterHandler(testHelper.EssentialDb, testHelper.MediatorMock.Object, testHelper.PasswordSecurity).Handle(command, default);

        testHelper.MediatorMock.Verify(mock => mock.Send(It.IsAny<VerifyRequestQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotEmpty(tokenDto.Payload.Token);
    }

    [Fact]
    public async void WhenRegisterExistingUser_ThrowException()
    {
        using var testHelper = new TestHelper();
        await testHelper.EssentialDb.Users.AddAsync(UserDbo.Create("email@email.com", "username", "password"));
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var user = await testHelper.EssentialDb.Users.FirstOrDefaultAsync();
        var command = new RegisterCommand( RequestDto.Create( RegisterDtoV1.Create(user.Email, user.Username, user.Password), "device",null, null));

        await Assert.ThrowsAsync<ConflictFailureException>(() => new RegisterCommand.RegisterHandler(testHelper.EssentialDb, testHelper.MediatorMock.Object, testHelper.PasswordSecurity).Handle(command, default));
    }

    [Theory]
    [InlineData(null, null, "password")]
    [InlineData("", "", "password")]
    [InlineData("username", "email", "")]
    public async void WhenInvalidInputs_ThrowException(string username, string email, string password)
    {
        using var testHelper = new TestHelper();
        var command = new RegisterCommand(RequestDto.Create( RegisterDtoV1.Create(email, username, password),"device", null, null));

        await Assert.ThrowsAnyAsync<ValidationException>(() => new RegisterCommand.RegisterHandler(testHelper.EssentialDb, testHelper.MediatorMock.Object, testHelper.PasswordSecurity).Handle(command, default));
    }
}