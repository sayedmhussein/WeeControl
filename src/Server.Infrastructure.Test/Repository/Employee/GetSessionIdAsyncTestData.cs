using System;
using System.Collections;
using System.Collections.Generic;
using MySystem.Web.EfRepository;
using Moq;
using MySystem.Shared.Library.Dbo;

namespace Web.Infrastructure.Test.Repository.Employee
{
    public class GetSessionIdAsyncTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new DataContext(ContextOptions.GetInMemoryOptions().Options), "", "password", "device", true, "Valid Credentials in MemoryDb" };
            yield return new object[] { new DataContext(ContextOptions.GetInMemoryOptions().Options), "username", null, "device", true, "Valid Credentials in MemoryDb" };

            yield return new object[] { new DataContext(ContextOptions.GetInMemoryOptions().Options), "username", "password", "device", false, "Valid Credentials in MemoryDb" };
            yield return new object[] { new DataContext(ContextOptions.GetInMemoryOptions().Options), "username1", "password", "device", true, "Invalid Credentials in MemoryDb" };

            yield return new object[] { new DataContext(ContextOptions.GetInMemoryOptions().Options), "hatem", "hatem", "device", true, "Invalid Lock Argument Not Null in Postges" };

            yield return new object[] { new DataContext(ContextOptions.GetPostgresOptions().Options), "username", "password", "device", false, "Invalid Credentials in Postges" };
            yield return new object[] { new DataContext(ContextOptions.GetPostgresOptions().Options), "username1", "password", "device", true, "Invalid Credentials in Postges" };

            
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
