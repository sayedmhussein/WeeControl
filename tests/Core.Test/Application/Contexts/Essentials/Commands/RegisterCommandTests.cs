using Moq;
using WeeControl.Core.Application.Contexts.Essentials.Commands;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DomainModel.Essentials.Dto;
using WeeControl.Core.SharedKernel.DtoParent;
using WeeControl.Core.SharedKernel.Exceptions;

namespace WeeControl.Core.Test.Application.Contexts.Essentials.Commands;

public class RegisterCommandTests
{
    [Fact]
    public async void WhenUserRegisterWithCustomer_ReturnSuccessAndToken()
    {
        using var testHelper = new CoreTestHelper();

        var tokenDto = await GetHandler(testHelper)
            .Handle(new UserRegisterCommand(GetUserProfileDto()), default);

        Assert.NotEmpty(tokenDto.Payload.Token);
        Assert.NotNull(testHelper.EssentialDb.Person.FirstOrDefault());
    }

    [Fact]
    public async void WhenRegisterNewUserWithCapital_DataInDbAreSmallLetters()
    {
        using var testHelper = new CoreTestHelper();
        var cmdDto = GetUserProfileDto();
        cmdDto.Payload.Email = cmdDto.Payload.Email.ToUpper();
        cmdDto.Payload.Username = cmdDto.Payload.Username.ToUpper();

        await GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default);

        var email = testHelper.EssentialDb.Person.First().Email.Where(char.IsLetter);
        Assert.True(email.All(char.IsLower));

        var username = testHelper.EssentialDb.Person.First().Username.Where(char.IsLetter);
        Assert.True(username.All(char.IsLower));
    }

    [Fact]
    public async void WhenRegisterNewUser_PasswordIsEncrypted()
    {
        using var testHelper = new CoreTestHelper();

        var cmdDto = GetUserProfileDto();

        await GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default);
        var savedPassword = testHelper.EssentialDb.Person.First().Password;

        Assert.NotEqual(cmdDto.Payload.Password, savedPassword);
    }

    [Fact]
    public async void WhenRegisterExistingUser_ThrowException()
    {
        using var testHelper = new CoreTestHelper();
        var cmdDto = GetUserProfileDto();

        await GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default);

        await Assert.ThrowsAsync<ConflictFailureException>(() =>
            GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default));
    }

    [Theory(Skip = "Merged user into person")]
    [InlineData(null, null, "password")]
    [InlineData("", "", "password")]
    [InlineData("username", "email", "")]
    public async void WhenInvalidInputs_ThrowException(string username, string email, string password)
    {
        using var testHelper = new CoreTestHelper();
        var cmdDto = GetUserProfileDto();
        cmdDto.Payload.Email = email;
        cmdDto.Payload.Username = username;
        cmdDto.Payload.Password = password;

        await Assert.ThrowsAnyAsync<EntityModelValidationException>(() =>
            GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default));
    }

    private static UserRegisterCommand.RegisterHandler GetHandler(CoreTestHelper testHelper)
    {
        testHelper.MediatorMock
            .Setup(x => x.Send(
                It.IsAny<SessionCreateCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResponseDto.Create(TokenResponseDto.Create("token")));

        return new UserRegisterCommand.RegisterHandler
        (
            testHelper.EssentialDb,
            testHelper.MediatorMock.Object,
            testHelper.PasswordSecurity
        );
    }

    private static RequestDto<UserProfileDto> GetUserProfileDto()
    {
        var dto = new UserProfileDto
        {
            FirstName = "FirstName", LastName = "LastName", NationalityCode = "EGP", DateOfBirth = DateTime.MinValue,
            Username = "test", Email = "test@test.com", Password = "123456"
        };

        return RequestDto.Create(dto, "device", 0, 0);
    }
}