using Microsoft.EntityFrameworkCore;
using WeeControl.Server.Domain.HumanResources;
using WeeControl.Server.Domain.HumanResources.Entities;
using WeeControl.Server.Persistence.HumanResources.Configurations;

namespace WeeControl.Server.Persistence.HumanResources
{
    public class HumanResourcesDbContext : DbContext, IHumanResourcesDbContext
    {
        public HumanResourcesDbContext(DbContextOptions<HumanResourcesDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Employee> Employees { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EmployeeEntityTypeConfiguration());
            
            
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(HumanResourcesDbContext).Assembly);
        }

        // private void SetBasicData()
        // {
        //     string territoryCode = "NYC-US" + new Random().NextDouble();
        //         
        //     if (Territories.Any()) return;
        //     
        //     var territory = Territory.Create(territoryCode,"Parent Territory", "USA", new Address(){ CityName = "New York"});
        //     Territories.Add(territory);
        //     SaveChanges();
        //
        //     if (Employees.Any()) return;
        //     
        //     var admin = Employee.Create(territoryCode, "Admin", "Admin", "admin", "admin");
        //     Employees.Add(admin);
        //     SaveChanges();
        //
        //     var employee = Employees.First();
        //     
        //     employee.Claims.Add(new Claims()
        //     {
        //         ClaimType = HumanResourcesData.Role, 
        //         ClaimValue = HumanResourcesData.Claims.Tags.SuperUser + ";test",
        //         GrantedBy = admin
        //     });
        //
        //     SaveChanges();
        // }
    }
}