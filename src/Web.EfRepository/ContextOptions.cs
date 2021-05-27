using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MySystem.Web.Infrastructure.EfRepository
{
    public static class ContextOptions
    {
        public static DbContextOptionsBuilder<DataContext> GetInMemoryOptions()
        {
            return GetInMemoryOptions(new Random().NextDouble().ToString());
        }

        public static DbContextOptionsBuilder<DataContext> GetInMemoryOptions(string dbName)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            return options;
        }

        public static DbContextOptionsBuilder<DataContext> GetPostgresOptions()
        {
            string connectionString;
#if DEBUG
            connectionString = "Server=127.0.0.1;Port=5432;Database=dbdatax;Username=sayed;Include Error Detail = true";
#else
            connectionString = "Server=127.0.0.1;Port=5432;Database=dbdatax;Username=sayed";
#endif
            return GetPostgresOptions(connectionString);
        }

        public static DbContextOptionsBuilder<DataContext> GetPostgresOptions(string connetionString)
        {
            var options = new DbContextOptionsBuilder<DataContext>();
            options.UseNpgsql(connetionString);
#if DEBUG
            options.EnableDetailedErrors();
#endif

            return options;
        }
    }
}
