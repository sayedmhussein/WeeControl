using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.Essential.Security;
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
            if (await context.Territories.AnyAsync(cancellationToken) == false)
            {
                await context.Territories.AddRangeAsync(GetTerritories(), cancellationToken);
            }

            await AddDeveloper(cancellationToken);

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

        private async Task AddDeveloper(CancellationToken cancellationToken)
        {
            await context.Users.AddAsync(UserDbo.Create("developer@weecontrol.com", 
                "developer", 
                passwordSecurity.Hash("developer"),
                "USA-HO"), cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var developer = await context.Users.Include(x => x.Claims).FirstAsync(x => x.Username == "developer", cancellationToken);
            developer.AddClaim(ClaimsTagsList.Claims.Developer, ClaimsTagsList.Tags.SuperUser, developer.UserId);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}