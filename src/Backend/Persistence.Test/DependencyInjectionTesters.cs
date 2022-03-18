// using System;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Moq;
// using WeeControl.Backend.Domain.Credentials;
// using WeeControl.Backend.Persistence;
// using Xunit;
//
// namespace WeeControl.Server.Persistence.Test
// {
//     public class DependencyInjectionTesters : IDisposable
//     {
//         private IServiceCollection services;
//         public DependencyInjectionTesters()
//         {
//             var configMock = new Mock<IConfiguration>();
//             configMock.Setup(x => x.GetSection("ConnectionStrings")["DatabaseProvider"]).Returns("Connection");
//
//             services = new ServiceCollection();
//         }
//         
//         public void Dispose()
//         {
//             services = null;
//         }
//         
//         [Fact]
//         public void WhenAddingPresistenceInMemory_ReturnMySystemDbContextObjectAsNotNull()
//         {
//             services.AddPersistenceAsInMemory();
//             
//             var service = services.BuildServiceProvider().GetService<ICredentialsDbContext>();
//
//             Assert.NotNull(service);
//         }
//
//         
//     }
// }
