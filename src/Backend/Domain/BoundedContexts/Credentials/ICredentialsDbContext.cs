using System;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Domain.Credentials.DatabaseObjects;
using WeeControl.Backend.Domain.Interfaces;

namespace WeeControl.Backend.Domain.Credentials
{
    public interface ICredentialsDbContext : IDbContext
    {
        DbSet<UserDbo> Users { get; set; }

        DbSet<SessionDbo> Sessions { get; set; }
    }
}
