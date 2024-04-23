using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.Domain.Contexts.Essentials;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.DomainModel.Essentials;
using WeeControl.Core.SharedKernel;

namespace WeeControl.Core.Application.Contexts.Developer;

public class SeedEssentialDatabaseCommand : IRequest
{
    public class SeedEssentialDatabaseHandler : IRequestHandler<SeedEssentialDatabaseCommand>
    {
        private readonly IEssentialDbContext context;
        private readonly IPasswordSecurity passwordSecurity;

        public SeedEssentialDatabaseHandler(IEssentialDbContext context, IPasswordSecurity passwordSecurity)
        {
            this.context = context;
            this.passwordSecurity = passwordSecurity;
        }

        public async Task Handle(SeedEssentialDatabaseCommand request, CancellationToken cancellationToken)
        {
            if (await context.Person.AnyAsync(cancellationToken))
                return;

            await context.Feeds.AddAsync(UserFeedsDbo.Create(
                "This is was injected from Seed (1)",
                "Body of feed", "/"), cancellationToken);
            await context.Feeds.AddAsync(UserFeedsDbo.Create(
                "This is was injected from Seed (2)",
                "Body 2 of feeds", "/"), cancellationToken);
            await context.Feeds.AddAsync(UserFeedsDbo.Create(
                "This is was injected from Seed(3)",
                "Body of feed", "/"), cancellationToken);

            //await context.Territories.AddRangeAsync(GetTerritories(), cancellationToken);

            var developerId = await AddPerson("developer", "EGP", new List<(string Type, string Value)>
            {
                (ClaimsValues.ClaimTypes.Developer, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Administrator, ClaimsValues.ClaimValues.Supervisor),
                (ClaimsValues.ClaimTypes.HumanResource, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Sales, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Director, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Field, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Finance, ClaimsValues.ClaimValues.SuperUser)
            }, cancellationToken);
            await AddEmployee(developerId, cancellationToken);

            var adminId = await AddPerson("admin", "EGP", new List<(string Type, string Value)>
            {
                (ClaimsValues.ClaimTypes.Administrator, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Administrator, ClaimsValues.ClaimValues.Manager)
            }, cancellationToken);
            await AddEmployee(adminId, cancellationToken);

            var customerId = await AddPerson("customer", "USA", null, cancellationToken);
            await AddCustomer(customerId, "EGP", cancellationToken);
        }

        private async Task<Guid> AddPerson(string name, string nationality,
            IEnumerable<(string Type, string Value)> claims, CancellationToken cancellationToken)
        {
            //var person = PersonDbo.Create(name, name, nationality, new DateTime(1999, 12, 31));
            var person = PersonDbo.Create(new PersonModel
            {
                FirstName = name,
                LastName = name,
                NationalityCode = nationality,
                DateOfBirth = new DateTime(2019, 12, 10),
                Username = name,
                Email = $"{name}@WeeControl.com",
                Password = passwordSecurity.Hash(name)
            });

            await context.Person.AddAsync(person, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            await context.PersonContacts
                .AddAsync(PersonContactDbo.Create(person.PersonId, ContactModel.ContactTypeEnum.Mobile, "+33567467646"),
                    cancellationToken);

            if (claims is not null && claims.Any())
            {
                var user1 = await context.Person.Include(x => x.Claims)
                    .FirstAsync(x => x.Username == name, cancellationToken);

                foreach (var c in claims)
                    await context.UserClaims.AddAsync(
                        UserClaimDbo.Create(user1.PersonId, c.Type, c.Value, user1.PersonId),
                        cancellationToken);

                await context.SaveChangesAsync(cancellationToken);
            }

            await context.UserNotifications.AddRangeAsync(new List<UserNotificationDbo>
            {
                UserNotificationDbo
                    .Create(person.PersonId, "User was created", "Body", "Link"),
                UserNotificationDbo
                    .Create(person.PersonId, "Second Notification", "Body", "Link"),
                UserNotificationDbo
                    .Create(person.PersonId, "Hello World", "Please confirm the visit", "www.github.com"),
                UserNotificationDbo
                    .Create(person.PersonId, "Check you inbox", ":)", "Link"),
                UserNotificationDbo
                    .Create(person.PersonId, "You have pending action", "You need to approve the job done.", "Link")
            }, cancellationToken);


            await context.SaveChangesAsync(cancellationToken);

            return person.PersonId;
        }

        private async Task AddEmployee(Guid userId, CancellationToken cancellationToken)
        {
            var employee = EmployeeDbo.Create(userId, null, new EmployeeModel {EmployeeNo = "12345"});
            await context.Employees.AddAsync(employee, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddCustomer(Guid userId, string country, CancellationToken cancellationToken)
        {
            var customer = CustomerDbo.Create(userId, new CustomerModel {CountryCode = country});
            await context.Customers.AddAsync(customer, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}