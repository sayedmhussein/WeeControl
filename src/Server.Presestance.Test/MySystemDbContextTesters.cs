using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySystem.Application.Common.Interfaces;
using Xunit;

namespace MySystem.Persistence.Test
{
    public class MySystemDbContextTesters
    {
        [Fact]
        public async void WhenQueryingTerritories_ListNotEmpty()
        {
            var context = new ServiceCollection().AddPersistenceAsInMemory(null).BuildServiceProvider().GetService<IMySystemDbContext>();

            var territories = await context.Territories.ToListAsync();

            Assert.NotEmpty(territories);
        }
    }
}
