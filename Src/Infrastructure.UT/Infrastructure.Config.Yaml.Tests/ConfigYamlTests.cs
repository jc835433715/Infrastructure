using NUnit.Framework;
using Infrastructure.Config.Yaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Config.Interface;

namespace Infrastructure.Config.Yaml.Tests
{
    [TestFixture()]
    public class ConfigYamlTests
    {
        [Test()]
        public void ConfigYamlTest()
        {
            var path =$"{ AppConfigHelper.GetCurrentDomainData(ConfigConst.AppDataDirectory)}";

            Assert.IsEmpty(path);
        }
    }
}