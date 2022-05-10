using System;
using System.Linq;
using Xunit;

namespace WeeControl.test.UserSecurityLib.Test.TestHelpers
{
    public class AttributeTester
    {
        public delegate TOutput MyDelegate<TEnum, TOutput>(TEnum msg);
        
        public void Test<TEnum>(MyDelegate<TEnum, string> myDelegate) where TEnum : Enum
        {
            foreach (var e in Enum.GetValues(typeof(TEnum)).Cast<TEnum>())
            {
                var item = myDelegate(e);

                Assert.False(string.IsNullOrEmpty(item), string.Format("\"{0}\" Enum don't have value in JSON file.", item));
            }
        }
    }
}