using System;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Essential;
using WeeControl.Application.Interfaces;
using WeeControl.Persistence;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.Services;

namespace WeeControl.Application.Test;

/// <summary>
/// Do the necessary setups for mocked objects, then create private field of handler.
/// </summary>
public abstract class ApplicationTestsBase : IDisposable
{
    protected readonly IJwtService JwtService;
    protected readonly IPasswordSecurity PasswordSecurity;
    protected IEssentialDbContext EssentialDb;
    protected Mock<IMediator> MediatorMock;
    protected Mock<IConfiguration> ConfigurationMock;
    protected Mock<ICurrentUserInfo> CurrentUserInfoMock;
    
    protected ApplicationTestsBase()
    {
        JwtService = new JwtService();
        PasswordSecurity = new PasswordSecurity();
        EssentialDb = new ServiceCollection()
            .AddPersistenceAsInMemory()
            .BuildServiceProvider()
            .GetService<IEssentialDbContext>();
        MediatorMock = new Mock<IMediator>();
        ConfigurationMock = new Mock<IConfiguration>();
        CurrentUserInfoMock = new Mock<ICurrentUserInfo>();
    }
    
    public void Dispose()
    {
        EssentialDb = null;
        MediatorMock = null;
        ConfigurationMock = null;
        CurrentUserInfoMock = null;
    }
}