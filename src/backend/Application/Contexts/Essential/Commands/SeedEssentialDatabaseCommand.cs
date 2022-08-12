using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Interfaces;
using WeeControl.Domain.Contexts.Essential;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.Entities;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Application.Contexts.Essential.Commands;

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

            await AddEmployee("developer", GetTerritories().First(x => x.UniqueName == "USA-HO").TerritoryId, new List<(string Type, string Value)>()
            {
                (ClaimsValues.ClaimTypes.Developer, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Administrator, ClaimsValues.ClaimValues.Supervisor),
                (ClaimsValues.ClaimTypes.HumanResource, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Sales, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Director, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Field, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Finance, ClaimsValues.ClaimValues.SuperUser)
            }, cancellationToken);
            
            await AddEmployee("admin", GetTerritories().First(x => x.UniqueName == "SAU-WEST").TerritoryId, new List<(string Type, string Value)>()
            {
                (ClaimsValues.ClaimTypes.Administrator, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Administrator, ClaimsValues.ClaimValues.Manager)
            }, cancellationToken);

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

        private async Task AddEmployee(string name, Guid territoryId, IEnumerable<(string Type, string Value)> claims, CancellationToken cancellationToken)
        {
            await context.Users.AddAsync(new UserDbo(new UserEntity()
            {
                Username = name,
                Email = $"{name}@WeeControl.com",
                MobileNo =  "+10"+ new Random().NextInt64(minValue:10000, maxValue:19999),
                Password = passwordSecurity.Hash(name)
            }), cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            if (claims is not null && claims.Any())
            {
                var user = await context.Users.Include(x => x.Claims)
                    .FirstAsync(x => x.Username == name, cancellationToken);
                
                foreach (var c in claims)
                {
                    await context.UserClaims.AddAsync(UserClaimDbo.Create(user.UserId, c.Type, c.Value, user.UserId), cancellationToken);
                }
                
                await context.SaveChangesAsync(cancellationToken);

                var employee = new EmployeeDbo(user.UserId, territoryId, new EmployeeEntity());
                await context.Employees.AddAsync(employee, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                var person = new PersonalEntity()
                {
                    FirstName = name, LastName = name, Nationality = "EGP"
                };
                await context.Person.AddAsync(new PersonDbo(user.UserId, person), cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}