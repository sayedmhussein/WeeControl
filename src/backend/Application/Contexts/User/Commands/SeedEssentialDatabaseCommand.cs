using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeeControl.Core.DataTransferObject.Temporary.Entities;
using WeeControl.Core.Domain.Contexts.Business;
using WeeControl.Core.Domain.Contexts.User;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.SharedKernel;
using WeeControl.Core.SharedKernel.Interfaces;

namespace WeeControl.Core.Application.Contexts.User.Commands;

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

        public async Task<Unit> Handle(SeedEssentialDatabaseCommand request, CancellationToken cancellationToken)
        {
            if (await context.Territories.AnyAsync(cancellationToken))
                return Unit.Value;

            await context.Territories.AddRangeAsync(GetTerritories(), cancellationToken);

            var developerId = await AddUser("developer", new List<(string Type, string Value)>()
            {
                (ClaimsValues.ClaimTypes.Developer, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Administrator, ClaimsValues.ClaimValues.Supervisor),
                (ClaimsValues.ClaimTypes.HumanResource, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Sales, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Director, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Field, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Finance, ClaimsValues.ClaimValues.SuperUser)
            }, cancellationToken);
            await AddPerson(developerId, "developer", "EGP", cancellationToken);
            await AddEmployee(developerId, GetTerritories().First(x => x.UniqueName == "USA-HO").TerritoryId,
                cancellationToken);

            var adminId = await AddUser("admin", new List<(string Type, string Value)>()
            {
                (ClaimsValues.ClaimTypes.Administrator, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Administrator, ClaimsValues.ClaimValues.Manager)
            }, cancellationToken);
            await AddPerson(adminId, "admin", "EGP", cancellationToken);
            await AddEmployee(adminId, GetTerritories().First(x => x.UniqueName == "SAU-WEST").TerritoryId,
                cancellationToken);

            var customerId = await AddUser("customer", null, cancellationToken);
            await AddPerson(customerId, "customer", "customer", cancellationToken);
            await AddCustomer(customerId, "EGP", cancellationToken);

            return Unit.Value;
        }

        private IEnumerable<TerritoryDbo> GetTerritories()
        {
            var usa = new TerritoryDbo("USA-HO", null, "USA");
            var egp = new TerritoryDbo("EGP-HO", null, "EGP", usa.TerritoryId);
            var cai = new TerritoryDbo("EGP-CAI", null, "EGP", egp.TerritoryId);
            var sau = new TerritoryDbo("SAU-HO", null, "SAU", usa.TerritoryId);
            var wst = new TerritoryDbo("SAU-WEST", null, "SAU", sau.TerritoryId);
            var jed = new TerritoryDbo("SAU-JED", null, "SAU", wst.TerritoryId);

            return new List<TerritoryDbo>() { usa, egp, cai, sau, wst, jed };
        }

        private async Task<Guid> AddUser(string name, IEnumerable<(string Type, string Value)> claims, CancellationToken cancellationToken)
        {
            var user = UserDbo.Create($"{name}@WeeControl.com", name,
                "+10" + new Random().NextInt64(minValue: 10000, maxValue: 19999), passwordSecurity.Hash(name));
            
            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            if (claims is not null && claims.Any())
            {
                var user1 = await context.Users.Include(x => x.Claims)
                    .FirstAsync(x => x.Username == name, cancellationToken);

                foreach (var c in claims)
                {
                    await context.UserClaims.AddAsync(UserClaimDbo.Create(user1.UserId, c.Type, c.Value, user1.UserId), cancellationToken);
                }

                await context.SaveChangesAsync(cancellationToken);
            }
            return user.UserId;
        }

        private async Task AddPerson(Guid userId, string name, string nationality, CancellationToken cancellationToken)
        {
            var person = PersonDbo.Create(userId,name, name, nationality, new DateOnly(1999, 12, 31));

            await context.Person.AddAsync(person, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddEmployee(Guid userId, Guid territoryId, CancellationToken cancellationToken)
        {
            var employee = new EmployeeDbo(userId, territoryId, "12345");
            await context.Employees.AddAsync(employee, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddCustomer(Guid userId, string country, CancellationToken cancellationToken)
        {
            var customer = new CustomerDbo(userId, country);
            await context.Customers.AddAsync(customer, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}