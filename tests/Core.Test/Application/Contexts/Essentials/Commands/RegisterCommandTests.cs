using Moq;
using WeeControl.Core.Application.Contexts.Essentials.Commands;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;

namespace WeeControl.Core.Test.Application.Contexts.Essentials.Commands;

public class RegisterCommandTests
{
    [Fact]
    public async void WhenRegisterNewUser_ReturnSuccessAndToken()
    {
        using var testHelper = new CoreTestHelper();
        testHelper.MediatorMock
            .Setup(x => x.Send(
                It.IsAny<SessionCreateCommand>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((ResponseDto<TokenResponseDto>) GetResponseDto());
        var cmdDto = GetRequestCommandDto();
        
        var tokenDto = await GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default);
        
        Assert.NotEmpty(tokenDto.Payload.Token);
    }
    
    [Fact]
    public async void WhenRegisterNewUserWithCapital_DataInDbAreSmallLetters()
    {
        using var testHelper = new CoreTestHelper();
        testHelper.MediatorMock
            .Setup(x => x.Send(
                It.IsAny<SessionCreateCommand>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetResponseDto());
        var cmdDto = GetRequestCommandDto();
        cmdDto.Payload.User.Email = cmdDto.Payload.User.Email.ToUpper();
        cmdDto.Payload.User.Username = cmdDto.Payload.User.Username.ToUpper();

        await GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default);
        
        var email = testHelper.EssentialDb.Users.First().Email.Where(char.IsLetter);
        Assert.True(email.All(char.IsLower));
        
        var username = testHelper.EssentialDb.Users.First().Username.Where(char.IsLetter);
        Assert.True(username.All(char.IsLower));
    }
    
    [Fact]
    public async void WhenRegisterNewUser_PasswordIsEncrypted()
    {
        using var testHelper = new CoreTestHelper();
        testHelper.MediatorMock
            .Setup(x => x.Send(
                It.IsAny<SessionCreateCommand>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync( GetResponseDto());
        var cmdDto = GetRequestCommandDto();

        await GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default);
        var savedPassword = testHelper.EssentialDb.Users.First().Password;

        Assert.NotEqual(cmdDto.Payload.User.Password, savedPassword);
    }

    [Fact]
    public async void WhenRegisterExistingUser_ThrowException()
    {
        using var testHelper = new CoreTestHelper();
        testHelper.MediatorMock
            .Setup(x => x.Send(
                It.IsAny<SessionCreateCommand>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetResponseDto());
        var cmdDto = GetRequestCommandDto();

        await GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default);

        await Assert.ThrowsAsync<ConflictFailureException>(() => GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default));
    }

    [Theory]
    [InlineData(null, null, "password")]
    [InlineData("", "", "password")]
    [InlineData("username", "email", "")]
    public async void WhenInvalidInputs_ThrowException(string username, string email, string password)
    {
        using var testHelper = new CoreTestHelper();
        var cmdDto = GetRequestCommandDto();
        cmdDto.Payload.User.Email = email;
        cmdDto.Payload.User.Username = username;
        cmdDto.Payload.User.Password = password;

        await Assert.ThrowsAnyAsync<ValidationException>(() => GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default));
    }

    private UserRegisterCommand.RegisterHandler GetHandler(CoreTestHelper testHelper)
    {
        return new UserRegisterCommand.RegisterHandler
        (
            testHelper.EssentialDb,
            testHelper.MediatorMock.Object,
            testHelper.PasswordSecurity
        );
    }
    
    private ResponseDto<TokenResponseDto> GetResponseDto()
    {
        return 
            ResponseDto.Create(TokenResponseDto.Create("token"));
    }
    
    private RequestDto<EmployeeRegisterDto> GetRequestCommandDto()
    {
        var dto = new EmployeeRegisterDto()
        {
            Person = { FirstName = "FirstName", LastName = "LastName", NationalityCode = "EGP", DateOfBirth = DateOnly.MinValue}
        };

        return RequestDto.Create(dto, "device", 0, 0);
    }
}