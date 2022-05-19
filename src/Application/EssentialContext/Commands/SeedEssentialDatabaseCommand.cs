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
            await context.ResetDatabaseAsync(cancellationToken);
            
            if (await context.Territories.AnyAsync(cancellationToken) == false)
            {
                foreach (var t in GetTerritores())
                {
                    await context.Territories.AddAsync(t, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                }
            }

            if (await context.Users.AnyAsync(cancellationToken) == false)
            {
                await context.Users.AddRangeAsync(GetUsers(), cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            var admin = await context.Users.FirstOrDefaultAsync(x => x.Username == "admin", cancellationToken);
            admin.TerritoryId = "USA-HO";
            await context.SaveChangesAsync(cancellationToken);

            if (await context.Claims.AnyAsync() == false)
            {
                await context.Claims.AddRangeAsync(GetClaims(admin.UserId), cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
            
            return Unit.Value;
        }

        private IEnumerable<TerritoryDbo> GetTerritores()
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

        private IEnumerable<UserDbo> GetUsers()
        {
            return new List<UserDbo>()
            {
                UserDbo.Create("admin@weecontrol.com", "admin", passwordSecurity.Hash("admin"), "USA-HO"),
                UserDbo.Create("user@weecontrol.com", "user", passwordSecurity.Hash("user"), "EGP-CAI")
            };
        }

        private IEnumerable<ClaimDbo> GetClaims(Guid adminId)
        {
            return new List<ClaimDbo>()
            {
                ClaimDbo.Create(adminId, ClaimsTagsList.Claims.Developer, ClaimsTagsList.Tags.SuperUser, adminId)
            };
        }
    }
}