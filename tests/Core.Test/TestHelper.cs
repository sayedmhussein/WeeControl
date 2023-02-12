using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using WeeControl.ApiApp.Persistence.DbContexts;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.Domain.Contexts.User;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.SharedKernel.Contexts.User;
using WeeControl.Core.SharedKernel.Interfaces;
using WeeControl.Core.SharedKernel.Services;

namespace WeeControl.Core.Test;

/// <summary>
/// Do the necessary setups for mocked objects, then create private field of handler.
/// </summary>
public class TestHelper : IDisposable
{
    public const string Email = "username@email.com";
    public const string Username = "username";
    public const string Password = "password";
    public const string DeviceId = "DeviceId";
    public const string MobileNo = "+1234567890";
    
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
    
    public (PersonModel Person, UserModel User, Guid personId, Guid userId, Guid sessionId) SeedDatabase()
    {
        var person = CreatePerson();
        var user = CreateUser(person.PersonId);
        var session = CreateSession(user.UserId);

        return (person, user, person.PersonId, user.UserId, session.SessionId);
    }

    private PersonDbo CreatePerson()
    {
        var personModel = new PersonModel()
        {
            FirstName = "First Name", SecondName = "Father Name", ThirdName = "Third Name", LastName = "Last Name",
            NationalityCode = "EGP", DateOfBirth = new DateOnly(1999, 12, 31)
        };
        var person = PersonDbo.Create(personModel);
        EssentialDb.Person.Add(person);
        EssentialDb.SaveChanges();

        return person;
    }

    private UserDbo CreateUser(Guid personId)
    {
        var userModel = new UserModel()
        {
            Email = Email, Username = Username,
            MobileNo = MobileNo, Password = PasswordSecurity.Hash(Password)
        };
        
        var user = UserDbo.Create(personId, userModel);
        EssentialDb.Users.Add(user);
        EssentialDb.SaveChanges();

        return user;
    }

    private UserSessionDbo CreateSession(Guid userId)
    {
        var session = UserSessionDbo.Create(userId, DeviceId, "0000");
        EssentialDb.UserSessions.Add(session);
        session.DisableOtpRequirement();
        EssentialDb.SaveChanges();
        return session;
    }
}