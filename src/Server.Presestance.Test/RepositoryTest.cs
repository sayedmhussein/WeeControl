using System;
using System.Linq;
using Xunit;

namespace MySystem.Presistance.Test
{
    public class RepositoryTest
    {
        [Fact]
        public void WhenFindingAll_ReturnListNotZero()
        {
            var optionsBuilder = ContextOptions.GetInMemoryOptions();
            var context = new DataContext(optionsBuilder.Options);
            var repo = new EmployeeRepo(context);

            var list = repo.FindAll();

            Assert.NotEmpty(list);
        }

        [Fact]
        public async void WhenAddingNewEmployee_CountMustIncreaed()
        {
            var optionsBuilder = ContextOptions.GetInMemoryOptions();
            var context = new DataContext(optionsBuilder.Options);
            var repo = new EmployeeRepo(context);

            var count1 = (await repo.FindAllAsync()).Count();

            await repo.InsertAsync(new EmployeeDbo() { FirstName = "bla", LastName = "bla", Username = "bla", Password = "bla" });

            var count2 = (await repo.FindAllAsync()).Count();
            Assert.True(count1 == count2 - 1);
        }

        [Fact]
        public async void WhenAddingNewOffice_NewIdMustBeCreated()
        {
            var optionsBuilder = ContextOptions.GetPostgresOptions();
            var context = new DataContext(optionsBuilder.Options);

            var name = new Random().NextDouble().ToString();
            var repo = new OfficeRepo(context);

            await repo.InsertAsync(new OfficeDbo() { CountryId = Country.SaudiArabia, OfficeName = name});

            var employee = (await repo.FindAllAsync()).FirstOrDefault(x => x.OfficeName == name);

            Assert.IsType<Guid>(employee.Id);
            Assert.NotEqual(Guid.Empty, employee.Id);
        }

        [Fact]
        public async void WhenDeletingEmployee_CountMustDecrease()
        {
            var optionsBuilder = ContextOptions.GetInMemoryOptions();
            var context = new DataContext(optionsBuilder.Options);
            var repo = new EmployeeRepo(context);

            var count1 = (await repo.FindAllAsync()).Count();

            var employee = repo.FindAll().FirstOrDefault();
            await repo.DeleteAsync(employee);

            var count2 = (await repo.FindAllAsync()).Count();
            Assert.True(count1 == count2 + 1);
        }

        [Fact]
        public async void WhenUpdatingEmployee_UpdateShouldBeSaved()
        {
            var optionsBuilder = ContextOptions.GetInMemoryOptions();
            var context = new DataContext(optionsBuilder.Options);
            var repo = new EmployeeRepo(context);

            var employee = repo.FindAll().FirstOrDefault();
            string oldName = employee.FirstName;
            string newName = new Random().NextDouble().ToString();

            employee.FirstName = newName;
            
            await repo.UpdateAsync(employee);

            var e = repo.Find(employee.Id);

            Assert.NotEqual(oldName, e.FirstName);
            Assert.Equal(newName, e.FirstName);
        }
    }

    public class EmployeeRepo : RepositoryBase<EmployeeDbo>
    {
        public EmployeeRepo(DataContext context) : base(context)
        {
        }
    }

    public class OfficeRepo : RepositoryBase<OfficeDbo>
    {
        public OfficeRepo(DataContext context) : base(context)
        {
        }
    }
}
