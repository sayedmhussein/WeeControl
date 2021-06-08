using System;
using Microsoft.Extensions.DependencyInjection;
using MySystem.Application.Common.Interfaces;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using Xunit;

namespace MySystem.Persistence.Test.Employee
{
    public class EmployeeDboTests
    {
        private readonly IMySystemDbContext context;

        public EmployeeDboTests()
        {
            context = new ServiceCollection().AddPersistenceAsInMemory(null).BuildServiceProvider().GetService<IMySystemDbContext>();
        }

        [Fact]
        public async void WhenAddingNewEmployee_EmployeeShouldHaveId()
        {
            var dbo = new EmployeeDbo() { };

            await context.Employees.AddAsync(dbo);
            await context.SaveChangesAsync(default);

            Assert.NotEqual(Guid.Empty, dbo.Id);
        }
    }
}
