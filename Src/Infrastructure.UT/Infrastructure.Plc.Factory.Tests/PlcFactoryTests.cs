using NUnit.Framework;
using Infrastructure.Plc.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Config.Interface;

namespace Infrastructure.Plc.Factory.Tests
{
    [TestFixture()]
    public class PlcFactoryTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            AppConfigHelper.SetCurrentDomainData(ConfigConst.AppDataDirectory, $@"{AppDomain.CurrentDomain.BaseDirectory  }\AppData");
        }


        [Test()]
        public void CreateTest()
        {
            //var plc = PlcFactory.Create();
        }
    }
}