using System;
using System.Threading;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.EssentialContext;
using WeeControl.Application.EssentialContext.Commands;
using WeeControl.Application.EssentialContext.Queries;
using WeeControl.Application.Exceptions;
using WeeControl.Domain.Essential.Entities;
using WeeControl.Persistence;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.SharedKernel.Services;
using Xunit;

namespace WeeControl.Application.Test.EssentialContext.Commands;

public class RegisterCommandTests : IDisposable
{
    private readonly IEssentialDbContext context;
    private Mock<IMediator> mediatorMock;
    private IPasswordSecurity passwordSecurity;

    public RegisterCommandTests()
    {
        context = new ServiceCollection().AddPersistenceAsInMemory().BuildServiceProvider().GetService<IEssentialDbContext>();
        mediatorMock = new Mock<IMediator>();

        passwordSecurity = new PasswordSecurity();
    }
        
    public void Dispose()
    {
        mediatorMock = null;
    }

    [Fact]
    public async void WhenRegisterNewUser_ReturnSuccessAndToken()
    {
        mediatorMock.Setup(x => x.Send(It.IsAny<GetNewTokenQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ResponseDto<TokenDtoV1>(new TokenDtoV1() { Token = "Token"}));
        var command = new RegisterCommand(new RequestDto("device"), RegisterDto.Create("email@emial.com", "username", "password"));
            
        var tokenDto = await new RegisterHandler(context, mediatorMock.Object, passwordSecurity).Handle(command, default);

        mediatorMock.Verify(mock => mock.Send(It.IsAny<VerifyRequestQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotEmpty(tokenDto.Token);
    }
    
    [Fact]
    public async void WhenRegisterNewUserWithCapital_DataInDbAreSmallLetters()
    {
        string email = "Email@someprovider.com";
        string username = "ThisIsUsername";
        mediatorMock.Setup(x => x.Send(It.IsAny<GetNewTokenQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ResponseDto<TokenDtoV1>(new TokenDtoV1() { Token = "Token"}));
        var command = new RegisterCommand(new RequestDto("device"), RegisterDto.Create(email.ToUpper(), username.ToUpper(), "password"));
            
        await new RegisterHandler(context, mediatorMock.Object, passwordSecurity).Handle(command, default);

        var byEmail = await context.Users.FirstOrDefaultAsync(x => x.Email == email.ToLower());
        Assert.NotNull(byEmail);
        
        var byUsername = await context.Users.FirstOrDefaultAsync(x => x.Username == username.ToLower());
        Assert.NotNull(byUsername);
    }
    
    [Fact]
    public async void WhenRegisterNewUser_PasswordIsEncrypted()
    {
        string postedPassword = "ThisIsPostedPassword";
        mediatorMock.Setup(x => x.Send(It.IsAny<GetNewTokenQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ResponseDto<TokenDtoV1>(new TokenDtoV1() { Token = "Token"}));
        var command = new RegisterCommand(new RequestDto("device"), RegisterDto.Create("email@emial.com", "username", "password"));
            
        await new RegisterHandler(context, mediatorMock.Object, passwordSecurity).Handle(command, default);

        var savedPassword = await context.Users.FirstOrDefaultAsync(x => x.Username == "username");

        Assert.NotEqual(postedPassword, savedPassword.Password);
    }
    
    [Fact]
    public async void WhenRegisterNewUser_VerifyingCorrectDeviceIsExecuted()
    {
        mediatorMock.Setup(x => x.Send(It.IsAny<GetNewTokenQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ResponseDto<TokenDtoV1>(new TokenDtoV1() { Token = "Token"}));
        var command = new RegisterCommand(new RequestDto("device"), RegisterDto.Create("email@emial.com", "username", "password"));
            
        var tokenDto = await new RegisterHandler(context, mediatorMock.Object, passwordSecurity).Handle(command, default);

        mediatorMock.Verify(mock => mock.Send(It.IsAny<VerifyRequestQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotEmpty(tokenDto.Token);
    }

    [Fact]
    public async void WhenRegisterExistingUser_ThrowException()
    {
        await context.Users.AddAsync(UserDbo.Create("email@email.com", "username", "password"));
        await context.SaveChangesAsync(default);
        var user = await context.Users.FirstOrDefaultAsync();
        var command = new RegisterCommand(new RequestDto("device"), RegisterDto.Create(user.Email, user.Username, user.Password));

        await Assert.ThrowsAsync<ConflictFailureException>(() => new RegisterHandler(context, mediatorMock.Object, passwordSecurity).Handle(command, default));
    }

    [Theory]
    [InlineData(null, null, "password")]
    [InlineData("", "", "password")]
    [InlineData("username", "email", "")]
    public async void WhenInvalidInputs_ThrowException(string username, string email, string password)
    {
        var command = new RegisterCommand(new RequestDto("device"), RegisterDto.Create(email, username, password));

        await Assert.ThrowsAnyAsync<ValidationException>(() => new RegisterHandler(context, mediatorMock.Object, passwordSecurity).Handle(command, default));
    }

        
}