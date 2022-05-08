// using System;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
// using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.Entities;
// using WeeControl.Backend.Persistence;
// using Xunit;
//
// namespace WeeControl.Server.Persistence.Test.BoundedContexts.HumanResources
// {
//     public class EmployeeDboTests : IDisposable
//     {
//         private IHumanResourcesDbContext context;
//
//         public EmployeeDboTests()
//         {
//             context = new ServiceCollection().AddPersistenceAsInMemory(nameof(EmployeeDboTests)).BuildServiceProvider().GetService<IHumanResourcesDbContext>();
//         }
//
//         public void Dispose()
//         {
//             context = null;
//         }
//
//         [Fact]
//         public async void WhenQueryingEmployees_ListNotEmpty()
//         {
//             var employees = await context.Employees.ToListAsync();
//
//             Assert.NotEmpty(employees);
//         }
//
//         [Fact]
//         public async void WhenAddingNewEmployee_EmployeeShouldHaveId()
//         {
//             var territoryCode = (await context.Territories.FirstAsync()).TerritoryCode;
//             var user = Employee.Create(territoryCode, "FirstName", "LastName", "user", "user");
//             
//             await context.Employees.AddAsync(user);
//             await context.SaveChangesAsync(default);
//             
//             Assert.NotEqual(Guid.Empty, user.EmployeeId);
//         }
//     }
// }
