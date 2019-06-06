using Infrastructure.Log.Factory;
using Infrastructure.Log.Interface;
using NUnit.Framework;

namespace Infrastructure.Utils.Tests
{
    [TestFixture()]
    public class LogManagerTests
    {
        [Test()]
        public void GetLoggerTest()
        {
            ILogger logger = LogManager.GetLogger<LogManagerTests>();

            Assert.IsNotNull(logger);
        }
    }
}