using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WeeControl.Server.Domain.HumanResources;
using WeeControl.Server.Domain.HumanResources.Entities;
using WeeControl.Server.Persistence.HumanResources;
using Xunit;

namespace WeeControl.Server.Persistence.Test.HumanResources
{
    public class EmployeeDboTests : IDisposable
    {
        private IHumanResourcesDbContext context;

        public EmployeeDboTests()
        {
            var options = new DbContextOptionsBuilder<HumanResourcesDbContext>();
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
            options.UseInMemoryDatabase(nameof(EmployeeDboTests));
            options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            
            context = new HumanResourcesDbContext(options: options.Options);
            //context = new ServiceCollection().AddPersistenceAsInMemory(nameof(EmployeeDboTests)).BuildServiceProvider().GetService<IHumanResourcesDbContext>();
        }

        public void Dispose()
        {
            context = null;
        }

        [Fact]
        public async void WhenQueryingEmployees_ListNotEmpty()
        {
            var employees = await context.Employees.ToListAsync();

            Assert.NotEmpty(employees);
        }

        [Fact]
        public async void WhenAddingNewEmployee_EmployeeShouldHaveId()
        {
            var user = Employee.Create("territoryCode", "FirstName", "LastName", "user", "user");
            
            await context.Employees.AddAsync(user);
            await context.SaveChangesAsync(default);
            
            Assert.NotEqual(Guid.Empty, user.EmployeeId);
        }
    }
}
