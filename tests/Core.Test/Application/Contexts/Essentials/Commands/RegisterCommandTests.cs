using Moq;
using WeeControl.Core.Application.Contexts.Essentials.Commands;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.SharedKernel;

namespace WeeControl.Core.Test.Application.Contexts.Essentials.Commands;

public class RegisterCommandTests
{
    [Fact]
    public async void WhenRegisterNewCustomer_ReturnSuccessAndToken()
    {
        using var testHelper = new CoreTestHelper();

        var tokenDto = await GetHandler(testHelper).Handle(new UserRegisterCommand(GetCustomerRegisterDto()), default);
        
        Assert.NotEmpty(tokenDto.Payload.Token);
        Assert.NotNull(testHelper.EssentialDb.Customers.FirstOrDefault());
    }
    
    [Fact]
    public async void WhenRegisterNewEmployee_ReturnSuccessAndToken()
    {
        using var testHelper = new CoreTestHelper();

        var tokenDto = await GetHandler(testHelper).Handle(new UserRegisterCommand(GetEmployeeRegisterDto()), default);
        
        Assert.NotEmpty(tokenDto.Payload.Token);
        Assert.NotNull(testHelper.EssentialDb.Employees.FirstOrDefault());
    }
    
    [Fact]
    public async void WhenRegisterNewUserWithCapital_DataInDbAreSmallLetters()
    {
        using var testHelper = new CoreTestHelper();
        var cmdDto = GetCustomerRegisterDto();
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

        var cmdDto = GetCustomerRegisterDto();

        await GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default);
        var savedPassword = testHelper.EssentialDb.Users.First().Password;

        Assert.NotEqual(cmdDto.Payload.User.Password, savedPassword);
    }

    [Fact]
    public async void WhenRegisterExistingUser_ThrowException()
    {
        using var testHelper = new CoreTestHelper();
        var cmdDto = GetCustomerRegisterDto();

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
        var cmdDto = GetCustomerRegisterDto();
        cmdDto.Payload.User.Email = email;
        cmdDto.Payload.User.Username = username;
        cmdDto.Payload.User.Password = password;

        await Assert.ThrowsAnyAsync<EntityModelValidationException>(() => GetHandler(testHelper).Handle(new UserRegisterCommand(cmdDto), default));
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

    private static RequestDto<CustomerRegisterDto> GetCustomerRegisterDto()
    {
        var dto = new CustomerRegisterDto()
        {
            Person = { FirstName = "FirstName", LastName = "LastName", NationalityCode = "EGP", DateOfBirth = DateOnly.MinValue},
            User = { Username = "test", Email = "test@test.com", Password = "123456", MobileNo = "+202222222" },
            Customer = { CountryCode = "EGP", CustomerName = "Customer Inc."}
        };

        return RequestDto.Create(dto, "device", 0, 0);
    }
    
    private static RequestDto<EmployeeRegisterDto> GetEmployeeRegisterDto()
    {
        var dto = new EmployeeRegisterDto()
        {
            Person = { FirstName = "FirstName", LastName = "LastName", NationalityCode = "EGP", DateOfBirth = DateOnly.MinValue},
            User = { Username = "test", Email = "test@test.com", Password = "123456", MobileNo = "+202222222" },
            Employee = { EmployeeNo = "1234", SupervisorEmployeeNo = string.Empty }
        };

        return RequestDto.Create(dto, "device", 0, 0);
    }
}