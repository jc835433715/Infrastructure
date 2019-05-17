using Infrastructure.Log.Interface;
using NUnit.Framework;

namespace Infrastructure.Log.NLog.Tests
{
    [TestFixture]
    class NLoggerFactoryTests
    {
        [Test]
        public void GetLoggerTest()
        {
            var logger = new NLoggerFactory().GetLogger<NLoggerFactoryTests>();

            Assert.IsNotNull(logger);
            Assert.IsInstanceOf<ILogger>(logger);
        }
    }
}
