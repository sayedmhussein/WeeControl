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

            await AddUser("developer", "USA-HO", new List<(string Type, string Value)>()
            {
                (ClaimsValues.ClaimTypes.Developer, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Administrator, ClaimsValues.ClaimValues.Supervisor),
                (ClaimsValues.ClaimTypes.HumanResource, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Sales, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Director, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Field, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Finance, ClaimsValues.ClaimValues.SuperUser)
            }, cancellationToken);
            
            await AddUser("admin", "SAU-WEST", new List<(string Type, string Value)>()
            {
                (ClaimsValues.ClaimTypes.Administrator, ClaimsValues.ClaimValues.SuperUser),
                (ClaimsValues.ClaimTypes.Administrator, ClaimsValues.ClaimValues.Manager)
            }, cancellationToken);
            
            await AddUser("HrUser", "SAU-WEST", new List<(string Type, string Value)>()
            {
                (ClaimsValues.ClaimTypes.HumanResource, ClaimsValues.ClaimValues.SuperUser)
            }, cancellationToken);

            return Unit.Value;
        }

        private IEnumerable<TerritoryDbo> GetTerritories()
        {
            return new List<TerritoryDbo>()
            {
                TerritoryDbo.Create("USA-HO", null, "USA", "Main Head Office"), 
                TerritoryDbo.Create("EGP-HO", "USA-HO", "EGP", "Main Head Office"),
                TerritoryDbo.Create("EGP-CAI", "EGP-HO", "EGP", "Cairo"),
                TerritoryDbo.Create("SAU-HO", "USA-HO", "SAU", "Saudi Arabia HO"),
                TerritoryDbo.Create("SAU-WEST", "SAU-HO", "SAU", "Western Region"),
                TerritoryDbo.Create("SAU-JED", "SAU-WEST", "SAU", "Jeddah")
            };
        }

        private async Task AddUser(string name, string territoryId, IEnumerable<(string Type, string Value)> claims, CancellationToken cancellationToken)
        {
            await context.Users.AddAsync(UserDbo.Create(
                name,
                name,
                $"{name}@WeeControl.com", 
                name, 
                passwordSecurity.Hash(name),
                "+10"+ new Random().NextInt64(minValue:10000, maxValue:19999),
                territoryId, "EGP"
                ), cancellationToken);
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
            }
            
        }
    }
}