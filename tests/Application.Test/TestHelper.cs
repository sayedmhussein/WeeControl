using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WeeControl.ApiApp.Persistence;
using WeeControl.ApiApp.Persistence.DbContexts;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.Domain.Contexts.User;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.SharedKernel.Interfaces;
using WeeControl.Core.SharedKernel.Services;

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

    private readonly SqliteConnection connection;
    private readonly EssentialDbContext context;

    public TestHelper()
    {
        JwtService = new JwtService();

        PasswordSecurity = new PasswordSecurity();

        connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<EssentialDbContext>()
            .UseSqlite(connection)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .Options;
        
        context = new EssentialDbContext(options);
        context.Database.EnsureCreated();

        EssentialDb = context;
        
        // EssentialDb = new ServiceCollection()
        //     .AddPersistenceAsSqlite()
        //     //.AddPersistenceAsInMemory()
        //     .BuildServiceProvider()
        //     .GetService<IEssentialDbContext>();
        
        

        MediatorMock = new Mock<IMediator>();

        ConfigurationMock = new Mock<IConfiguration>();

        CurrentUserInfoMock = new Mock<ICurrentUserInfo>();
    }

    public void Dispose()
    {
        connection.Dispose();
        context.Dispose();
        EssentialDb = null;
        MediatorMock = null;
        ConfigurationMock = null;
        CurrentUserInfoMock = null;
    }

    public UserDbo GetUserDboWithEncryptedPassword(string username, string password, string territory = "TST")
    {
        
        return UserDbo.Create(Guid.NewGuid(),username + "@email.com", username, "0123456789", PasswordSecurity.Hash(password));
    }
}