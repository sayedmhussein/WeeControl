using System;
using Microsoft.EntityFrameworkCore;
using MySystem.Web.EfRepository;
using Xunit;

namespace Web.EfRepository.Test
{
    public class DataContextTests
    {
        [Fact]
        public void WhenCreatingDataContextWithInvalidConnectionString_ThrowArgumentException()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseNpgsql("dfdf");

            Assert.Throws<ArgumentException>(() => new DataContext(optionsBuilder.Options));
        }

        [Fact]
        public void WhenCreatingDataContextWithLocalPostgres_ContextNotNull()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=dbdatax;Username=sayed;Include Error Detail = true");

            var context = new DataContext(optionsBuilder.Options);

            Assert.NotNull(context);
        }

        [Fact]
        public async void WhenCreatingDataContextWithLocalPostgresAndGetFirstOffice_OfficeNotNull()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=dbdatax;Username=sayed;Include Error Detail = true");
            var context = new DataContext(optionsBuilder.Options);

            var office = await context.Offices.FirstOrDefaultAsync();

            Assert.NotNull(office);
            Assert.IsType<Guid>(office.Id);
            Assert.NotEqual(Guid.Empty, office.Id);
        }
    }
}
