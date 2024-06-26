using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using WeeControl.ApiApp.Persistence.DbContexts;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.Application.Services;
using WeeControl.Core.Domain.Contexts.Essentials;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.DomainModel.Essentials;
using WeeControl.Core.SharedKernel.Interfaces;
using WeeControl.Core.SharedKernel.Services;

namespace WeeControl.Core.Test;

/// <summary>
///     Do the necessary setups for mocked objects, then create private field of handler.
/// </summary>
public class CoreTestHelper : IDisposable
{
    public const string Email = "username@email.com";
    public const string Username = "username";
    public const string Password = "password";
    public const string DeviceId = "DeviceId";
    public const string MobileNo = "+1234567890";
    public const string ClaimTypeExample = "ClaimTypeExample";
    public const string ClaimValueExample = "ClaimValueExample";

    private readonly SqliteConnection connection;
    private readonly EssentialDbContext context;

    public readonly IJwtService JwtService;
    public readonly IPasswordSecurity PasswordSecurity;
    public Mock<IConfiguration> ConfigurationMock;
    public Mock<ICurrentUserInfo> CurrentUserInfoMock;
    public IEssentialDbContext EssentialDb;
    public Mock<IMediator> MediatorMock;

    public CoreTestHelper()
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
        EssentialDb = null!;
        MediatorMock = null!;
        ConfigurationMock = null!;
        CurrentUserInfoMock = null!;
    }

    public (PersonModel Person, Guid personId, Guid sessionId) SeedDatabase(
        string? username = null)
    {
        return SeedDatabase(EssentialDb, username);
    }

    public static (PersonModel Person, Guid personId, Guid sessionId) SeedDatabase(
        IEssentialDbContext dbContext, string? username = null)
    {
        var person = CreatePerson(dbContext, username);
        var session = CreateSession(dbContext, person.PersonId);

        return (person, person.PersonId, session.SessionId);
    }

    private static PersonDbo CreatePerson(IEssentialDbContext dbContext, string? username)
    {
        var personModel = new PersonModel
        {
            FirstName = username ?? "First Name", SecondName = "Father Name", ThirdName = "Third Name",
            LastName = username ?? "Last Name",
            NationalityCode = "EGP", DateOfBirth = new DateTime(1999, 12, 31),
            Email = Email, Username = username ?? Username,
            Password = new PasswordSecurity().Hash(username ?? Password)
        };

        if (username is not null) personModel.Email = $"{username}@{username}.com";

        var person = PersonDbo.Create(personModel);
        dbContext.Person.Add(person);
        dbContext.SaveChanges();

        dbContext.PersonContacts.Add(PersonContactDbo.Create(person.PersonId, ContactModel.ContactTypeEnum.Mobile,
            MobileNo));
        dbContext.SaveChanges();

        dbContext.UserNotifications.Add(UserNotificationDbo.Create(person.PersonId, "Subject 1",
            $"Created at {DateTime.Now}", ""));
        dbContext.UserNotifications.Add(UserNotificationDbo.Create(person.PersonId, "Subject 2",
            $"Created at {DateTime.Now}", ""));
        dbContext.UserNotifications.Add(UserNotificationDbo.Create(person.PersonId, "Subject 3",
            $"Created at {DateTime.Now}", ""));

        if (username is null)
        {
            dbContext.Feeds.Add(UserFeedsDbo.Create("Feed Subject 1", "Feed body 1", ""));
            dbContext.Feeds.Add(UserFeedsDbo.Create("Feed Subject 2", "Feed body 2", ""));
        }

        dbContext.UserClaims.Add(UserClaimDbo.Create(person.PersonId, ClaimTypeExample, ClaimValueExample,
            person.PersonId));

        dbContext.SaveChanges();
        return person;
    }

    private static UserSessionDbo CreateSession(IEssentialDbContext dbContext, Guid userId)
    {
        var session = UserSessionDbo.Create(userId, DeviceId, "0000");
        dbContext.UserSessions.Add(session);
        session.DisableOtpRequirement();
        dbContext.SaveChanges();
        return session;
    }
}