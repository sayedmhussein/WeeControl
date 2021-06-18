using System;
using System.Net.Http;
using Moq;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.Applications.BaseLib.Services;
using Xunit;

namespace WeeControl.User.Employee.Test.UnitTests.Services
{
    public class ViewModelDependencyFactoryTesters
    {
        [Fact]
        public void Constructor_WhenHttpClientIsNull_ThrowArgumentNullException()
        {
            Action action = new(() => new ViewModelDependencyFactory(null, new Mock<IDevice>().Object, "path"));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void Constructor_WhenDeviceIsNull_ThrowArgumentNullException()
        {
            Action action = new(() => new ViewModelDependencyFactory(new HttpClient(), null, "path"));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void Constructor_WhenDataPathtIsNull_ThrowArgumentNullException()
        {
            Action action = new(() => new ViewModelDependencyFactory(new HttpClient(), new Mock<IDevice>().Object, null));

            Assert.Throws<ArgumentNullException>(action);
        }
    }
}
