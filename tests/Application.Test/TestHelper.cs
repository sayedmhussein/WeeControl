using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WeeControl.ApiApp.Persistence;
using WeeControl.ApiApp.Persistence.DbContexts;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.Domain.Contexts.User;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.SharedKernel.Contexts.User;
using WeeControl.Core.SharedKernel.Interfaces;
using WeeControl.Core.SharedKernel.Services;

namespace WeeControl.ApiApp.Application.Test;

/// <summary>
/// Do the necessary setups for mocked objects, then create private field of handler.
/// </summary>
public class TestHelper : IDisposable
{
    public const string Email = "username@email.com";
    public const string Username = "username";
    public const string Password = "password";
    public const string DeviceId = "DeviceId";
    
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

    public (PersonModel Person, UserModel User, Guid personId, Guid userId, Guid sessionId) SeedDatabase()
    {
        var personModel = new PersonModel()
        {
            FirstName = "First Name", SecondName = "Father Name", ThirdName = "Third Name", LastName = "Last Name",
            NationalityCode = "EGP", DateOfBirth = new DateOnly(1999, 12, 31)
        };
        var person = PersonDbo.Create(personModel);
        EssentialDb.Person.Add(person);
        EssentialDb.SaveChanges();

        var userModel = new UserModel()
        {
            Email = Email, Username = Username,
            MobileNo = "0123456789", Password = PasswordSecurity.Hash(Password)
        };
        var user = UserDbo.Create(person.PersonId, userModel);
        EssentialDb.Users.Add(user);
        EssentialDb.SaveChanges();

        var session = UserSessionDbo.Create(user.UserId, DeviceId, "0000");
        EssentialDb.UserSessions.Add(session);
        session.DisableOtpRequirement();
        EssentialDb.SaveChanges();

        return (personModel, userModel, person.PersonId, user.UserId, session.SessionId);
    }

    [Obsolete("Use returned value from seed")]
    public Guid GetUserId(string username = Username)
    {
        return EssentialDb.Users.First(x => x.Username == username).UserId;
    }
}