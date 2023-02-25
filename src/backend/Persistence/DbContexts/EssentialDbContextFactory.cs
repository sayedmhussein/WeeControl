using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WeeControl.ApiApp.Persistence.DbContexts;

public class EssentialDbContextFactory : IDesignTimeDbContextFactory<EssentialDbContext>
{
    public EssentialDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EssentialDbContext>();
        optionsBuilder.UseMySQL("Server=127.0.0.1;Port=3306;Database=essentialDb;user=sayed;password=sayed");

        return new EssentialDbContext(optionsBuilder.Options);
    }
}