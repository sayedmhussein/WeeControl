using System;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Essential;
using WeeControl.Application.Interfaces;
using WeeControl.Domain.Contexts.Essential;
using WeeControl.Persistence;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.Services;

namespace WeeControl.Application.Test.Essential;

/// <summary>
/// Do the necessary setups for mocked objects, then create private field of handler.
/// </summary>
public class TestHelper : IDisposable
{
    public readonly IJwtService JwtService;
    
    [Obsolete("Use function with encrypted password instead.")]
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
    
    [Obsolete("Use function with encrypted password")]
    public UserDbo GetUserDbo(string username, string password, string territory = "TST")
    {
        return UserDbo.Create(
            nameof(UserDbo.FirstName), 
            nameof(UserDbo.LastName), 
            (username + "@email.com").ToLower(), 
            username.ToLower(),
            password, 
            "012345667", 
            territory, "EGP");
    }
    
    public UserDbo GetUserDboWithEncryptedPassword(string username, string password, string territory = "TST")
    {
        return UserDbo.Create(
            nameof(UserDbo.FirstName), 
            nameof(UserDbo.LastName), 
            (username + "@email.com").ToLower(), 
            username.ToLower(),
            PasswordSecurity.Hash(password), 
            "012345667", 
            territory, "EGP");
    }
}