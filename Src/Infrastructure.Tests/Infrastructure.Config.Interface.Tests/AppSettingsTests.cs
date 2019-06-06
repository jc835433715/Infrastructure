using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Config.Interface.Tests
{
    [TestFixture()]
    public class AppSettingsTests
    {
        [Test()]
        public void GetTest()
        {
            var value = AppConfigHelper.AppSettings.Get("key");

            AppConfigHelper.AppSettings.Set("key", "value");

            Assert.AreEqual("value", value);
        }
    }
}