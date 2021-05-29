using System;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MySystem.Presistance.Test
{
    public class DataContextTests
    {
        [Fact]
        public void WhenCreatingDataContextWithInvalidConnectionString_ThrowArgumentException()
        {
            var options = ContextOptions.GetPostgresOptions("blabla");

            Assert.Throws<ArgumentException>(() => new DataContext(options.Options));
        }

        [Fact]
        public void WhenCreatingDataContextWithLocalPostgres_ContextNotNull()
        {
            var optionsBuilder = ContextOptions.GetPostgresOptions();

            var context = new DataContext(optionsBuilder.Options);

            Assert.NotNull(context);
        }

        [Fact]
        public async void WhenCreatingDataContextWithLocalPostgresAndGetFirstOffice_OfficeNotNull()
        {
            var optionsBuilder = ContextOptions.GetInMemoryOptions();
            var context = new DataContext(optionsBuilder.Options);

            var office = await context.Offices.FirstOrDefaultAsync();

            Assert.NotNull(office);
            Assert.IsType<Guid>(office.Id);
            Assert.NotEqual(Guid.Empty, office.Id);
        }
    }
}
