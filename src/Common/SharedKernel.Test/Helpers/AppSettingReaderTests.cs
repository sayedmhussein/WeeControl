using System;
using System.Collections.Generic;
using System.Reflection;
using WeeControl.SharedKernel.Helpers;
using WeeControl.SharedKernel.Obsolutes;
using Xunit;

namespace WeeControl.SharedKernel.Test.Helpers
{
    public class AppSettingReaderTests : IDisposable
    {
        private Dictionary<Parent, string> dict = null;
        private readonly AppSettingReader settingReader;
        public AppSettingReaderTests()
        {
            settingReader = new AppSettingReader(
                typeof(AppSettingReaderTests).Namespace, 
                "appsettings.json", 
                Assembly.GetExecutingAssembly());
        }
        
        public void Dispose()
        {
            dict = null;
        }
        
        [Fact]
        public void JsonFileIsNotNull()
        {
            settingReader.PopulateAttribute(ref dict, "Parent");

            Assert.NotNull(dict);
        }

        [Fact]
        public void JsonFileIsNotEmpty()
        {
            settingReader.PopulateAttribute(ref dict, "Parent");
            
            Assert.NotEmpty(dict);
        }
        
        [Fact]
        public void EnumValueIsCorrect()
        {
            settingReader.PopulateAttribute(ref dict, "Parent");

            var value = dict[Parent.Child];
            
            Assert.Equal("Value", value);
        }
        
        private enum Parent
        {
            Child
        }
    }
}
