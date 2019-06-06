using Infrastructure.Config.Interface;
using NUnit.Framework;
using System;

namespace Infrastructure.Auth.Imp.Tests
{
    [TestFixture()]
    public class AuthImpTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            AppConfigHelper.SetCurrentDomainData(ConfigConst.DataDirectory, $@"{AppDomain.CurrentDomain.BaseDirectory  }\AppData");
        }

        [Test()]
        public void LoginTest()
        {
            var authImp = new AuthImp();

            authImp.Login("YHY", "123456");
        }

        [Test()]
        public void HasRightTest()
        {
            var authImp = new AuthImp();

            authImp.HasRight("设置");
        }
    }
}