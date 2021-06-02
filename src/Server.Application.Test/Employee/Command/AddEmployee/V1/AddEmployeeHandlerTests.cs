using System;
using Application.Employee.Command.CreateEmployee.V1;
using Microsoft.Extensions.DependencyInjection;
using MySystem.Application.Common.Interfaces;
using MySystem.Application.Employee.Command.AddEmployee.V1;
using MySystem.Persistence;
using MySystem.SharedKernel.Entities.Employee.V1Dto;
using Xunit;
namespace MySystem.Application.Test.Employee.Command.AddEmployee.V1
{
    public class AddEmployeeHandlerTests
    {
        private IMySystemDbContext dbContext;

        public AddEmployeeHandlerTests()
        {
            dbContext = new ServiceCollection().AddPersistenceAsInMemory(null).BuildServiceProvider().GetService<IMySystemDbContext>();
        }

        [Fact]
        public async void WhenAddingNewEmployeeWithUniqueData_ReturnSameEmployee()
        {
            var dto = new EmployeeDto() { };
            AddEmployeeCommand command = new() { DeviceId = "-", Payload = dto };

            var response = await new AddEmployeeHandler(dbContext, null).Handle(command, default);

            Assert.NotEqual(Guid.Empty, response.Payload.Id);
        }
    }
}
