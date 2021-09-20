using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WeeControl.Backend.Domain.Common.Interfaces;
using WeeControl.Backend.Domain.EntityGroups.Employee;
using WeeControl.Backend.Domain.EntityGroups.Territory;
using WeeControl.Common.SharedKernel.EntityGroups.Employee.Attributes;
using WeeControl.Common.SharedKernel.EntityGroups.Territory.Attributes;

namespace WeeControl.Backend.Persistence
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }

    public sealed class MySystemDbContext : DbContext, IMySystemDbContext
    {
        internal static DatabaseFacade DbFacade { get; private set; }

        public MySystemDbContext(DbContextOptions<MySystemDbContext> options) : base(options)
        {
            DbFacade = Database;

            Database.EnsureCreated();
            if (!Territories.Any())
            {
                DbInitialization dbInitialization = new(this);
                dbInitialization.Init(new TerritoryAppSetting(), new EmployeeAttribute());
            }
        }

        //Territory Schema
        public DbSet<TerritoryDbo> Territories { get; set; }

        //Employee Schema
        public DbSet<EmployeeDbo> Employees { get; set; }
        //
        public DbSet<EmployeeClaimDbo> EmployeeClaims { get; set; }
        public DbSet<EmployeeIdentityDbo> EmployeeIdentities { get; set; }
        //
        public DbSet<EmployeeSessionDbo> EmployeeSessions { get; set; }
        public DbSet<EmployeeSessionLogDbo> EmployeeSessionLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var configuration = new DbConfiguration(Database);
            configuration.Configure(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MySystemDbContext).Assembly);
        }
    }
}
