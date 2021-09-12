using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace WeeControl.Backend.Persistence
{
    public class DbConfiguration
    {
        private readonly DatabaseFacade databaseFacade;

        public DbConfiguration(DatabaseFacade databaseFacade)
        {
            this.databaseFacade = databaseFacade;
        }

        public void Configure(ModelBuilder modelBuilder)
        {
            if (databaseFacade.IsNpgsql())
            {
                modelBuilder.HasPostgresExtension("uuid-ossp");

                foreach (var entity in modelBuilder.Model.GetEntityTypes())
                {        
                    foreach (var property in entity.GetProperties())
                    {
                        property.SetColumnName(property.Name.ToSnakeCase());
                    }

                    foreach (var key in entity.GetKeys())
                    {
                        key.SetName(key.GetName().ToSnakeCase());
                    }

                    foreach (var key in entity.GetForeignKeys())
                    {
                        key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                    }

                    foreach (var index in entity.GetIndexes())
                    {
                        index.SetDatabaseName(index.Name.ToSnakeCase());
                    }
                }
            }
        }
    }
}