using Infrastructure.Config.Interface;
using NUnit.Framework;
using System.Configuration;

namespace Infrastructure.Config.Tests
{
    [TestFixture()]
    public class ConnectionStringsTests
    {
        [Test()]
        public void Test()
        {
            var connectionStrings = AppConfigHelper.ConnectionStrings.Get("UT");

            AppConfigHelper.ConnectionStrings.Set("UT", new ConnectionStringSettings("UT", " 2 ", "3"));
        }
    }
}