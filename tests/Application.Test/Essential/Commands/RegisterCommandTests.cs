using System.Linq;
using System.Threading;
using Moq;
using WeeControl.Application.Contexts.Essential.Commands;
using WeeControl.Application.Contexts.Essential.Queries;
using WeeControl.Application.Exceptions;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;
using Xunit;

namespace WeeControl.Application.Test.Essential.Commands;

public class RegisterCommandTests
{
    [Fact]
    public async void WhenRegisterNewUser_ReturnSuccessAndToken()
    {
        using var testHelper = new TestHelper();
        testHelper.MediatorMock
            .Setup(x => x.Send(
                It.IsAny<UserTokenQuery>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((ResponseDto<TokenDtoV1>) GetResponseDto());
        var cmdDto = GetRequestCommandDto();
        
        var tokenDto = await GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default);
        
        Assert.NotEmpty(tokenDto.Payload.Token);
    }
    
    [Fact]
    public async void WhenRegisterNewUserWithCapital_DataInDbAreSmallLetters()
    {
        using var testHelper = new TestHelper();
        testHelper.MediatorMock
            .Setup(x => x.Send(
                It.IsAny<UserTokenQuery>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((ResponseDto<TokenDtoV1>) GetResponseDto());
        var cmdDto = GetRequestCommandDto();
        cmdDto.Payload.Email = cmdDto.Payload.Email.ToUpper();
        cmdDto.Payload.Username = cmdDto.Payload.Username.ToUpper();

        await GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default);
        
        var email = testHelper.EssentialDb.Users.First().Email.Where(char.IsLetter);
        Assert.True(email.All(char.IsLower));
        
        var username = testHelper.EssentialDb.Users.First().Username.Where(char.IsLetter);
        Assert.True(username.All(char.IsLower));
    }
    
    [Fact]
    public async void WhenRegisterNewUser_PasswordIsEncrypted()
    {
        using var testHelper = new TestHelper();
        testHelper.MediatorMock
            .Setup(x => x.Send(
                It.IsAny<UserTokenQuery>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((ResponseDto<TokenDtoV1>) GetResponseDto());
        var cmdDto = GetRequestCommandDto();

        await GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default);
        var savedPassword = testHelper.EssentialDb.Users.First().Password;

        Assert.NotEqual(cmdDto.Payload.Password, savedPassword);
    }

    [Fact]
    public async void WhenRegisterExistingUser_ThrowException()
    {
        using var testHelper = new TestHelper();
        testHelper.MediatorMock
            .Setup(x => x.Send(
                It.IsAny<UserTokenQuery>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((ResponseDto<TokenDtoV1>) GetResponseDto());
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
        using var testHelper = new TestHelper();
        var cmdDto = GetRequestCommandDto();
        cmdDto.Payload.Email = email;
        cmdDto.Payload.Username = username;
        cmdDto.Payload.Password = password;

        await Assert.ThrowsAnyAsync<ValidationException>(() => GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default));
    }

    private UserRegisterCommand.RegisterHandler GetHandler(TestHelper testHelper)
    {
        return new UserRegisterCommand.RegisterHandler
        (
            testHelper.EssentialDb,
            testHelper.MediatorMock.Object,
            testHelper.PasswordSecurity
        );
    }
    
    private IResponseDto<TokenDtoV1> GetResponseDto()
    {
        return 
            ResponseDto.Create(TokenDtoV1.Create("token", "name", "url"));
    }
    
    private IRequestDto<UserRegisterDto> GetRequestCommandDto()
    {
        var dto = new UserRegisterDto()
        {
            TerritoryId = "TRR",
            FirstName = nameof(IUserModel.FirstName),
            LastName = nameof(IUserModel.LastName),
            Email = nameof(IUserModel.Email) + "@email.com",
            Username = nameof(IUserModel.Username),
            Password = nameof(IUserModel.Password),
            MobileNo = "0123456789",
            Nationality = "EGP"
        };

        return RequestDto.Create(dto, "device", 0, 0);
    }
}