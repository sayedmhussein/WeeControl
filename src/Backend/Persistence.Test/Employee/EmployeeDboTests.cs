using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Domain.BasicDbos.EmployeeSchema;
using WeeControl.Backend.Domain.Interfaces;
using Xunit;

namespace WeeControl.Backend.Persistence.Test.Employee
{
    public class EmployeeDboTests
    {
        private readonly IMySystemDbContext context;

        public EmployeeDboTests()
        {
            context = new ServiceCollection().AddPersistenceAsInMemory(null).BuildServiceProvider().GetService<IMySystemDbContext>();
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
            var dbo = new EmployeeDbo() { };

            await context.Employees.AddAsync(dbo);
            await context.SaveChangesAsync(default);

            Assert.NotEqual(Guid.Empty, dbo.Id);
        }
    }
}
