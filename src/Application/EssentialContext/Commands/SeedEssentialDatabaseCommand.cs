using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Interfaces;

namespace WeeControl.Application.EssentialContext.Commands;

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
                (ClaimsTagsList.Claims.Developer, ClaimsTagsList.Tags.SuperUser),
                (ClaimsTagsList.Claims.Administrator, ClaimsTagsList.Tags.Supervisor),
                (ClaimsTagsList.Claims.HumanResource, ClaimsTagsList.Tags.SuperUser),
                (ClaimsTagsList.Claims.Sales, ClaimsTagsList.Tags.SuperUser),
                (ClaimsTagsList.Claims.Director, ClaimsTagsList.Tags.SuperUser),
                (ClaimsTagsList.Claims.Field, ClaimsTagsList.Tags.SuperUser),
                (ClaimsTagsList.Claims.Finance, ClaimsTagsList.Tags.SuperUser)
            }, cancellationToken);
            
            await AddUser("admin", "SAU-WEST", new List<(string Type, string Value)>()
            {
                (ClaimsTagsList.Claims.Administrator, ClaimsTagsList.Tags.SuperUser),
                (ClaimsTagsList.Claims.Administrator, ClaimsTagsList.Tags.Manager)
            }, cancellationToken);
            
            await AddUser("HrUser", "SAU-WEST", new List<(string Type, string Value)>()
            {
                (ClaimsTagsList.Claims.HumanResource, ClaimsTagsList.Tags.SuperUser)
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
            await context.Users.AddAsync(UserDbo.Create($"{name}@WeeControl.com", 
                name, 
                passwordSecurity.Hash(name),
                territoryId), cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            if (claims is not null && claims.Any())
            {
                var user = await context.Users.Include(x => x.Claims)
                    .FirstAsync(x => x.Username == name, cancellationToken);
                
                foreach (var c in claims)
                {
                    await context.Claims.AddAsync(ClaimDbo.Create(user.UserId, c.Type, c.Value, user.UserId), cancellationToken);
                }
                
                await context.SaveChangesAsync(cancellationToken);
            }
            
        }
    }
}