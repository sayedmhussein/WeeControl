using System;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.ApiApp.Application.Interfaces;
using WeeControl.ApiApp.Domain.Contexts.Essential;
using WeeControl.ApiApp.Persistence;
using WeeControl.Common.SharedKernel.Contexts.Temporary.Entities;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Services;

namespace WeeControl.ApiApp.Application.Test;

/// <summary>
/// Do the necessary setups for mocked objects, then create private field of handler.
/// </summary>
public class TestHelper : IDisposable
{
    public readonly IJwtService JwtService;
    public readonly IPasswordSecurity PasswordSecurity;
    public IEssentialDbContext EssentialDb;
    public Mock<IMediator> MediatorMock;
    public Mock<IConfiguration> ConfigurationMock;
    public Mock<ICurrentUserInfo> CurrentUserInfoMock;
    
    public TestHelper()
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

    public UserDbo GetUserDboWithEncryptedPassword(string username, string password, string territory = "TST")
    {
        var entity = new UserEntity()
        {
            Username = username,
            Email = username + "@email.com",
            Password = PasswordSecurity.Hash(password),
            MobileNo = "0123456789"
        };
        
        return new UserDbo(entity);
    }
}