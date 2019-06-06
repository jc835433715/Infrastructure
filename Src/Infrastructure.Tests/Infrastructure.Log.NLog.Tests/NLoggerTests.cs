using NUnit.Framework;
using NSubstitute;
using NLog;
using System;

namespace Infrastructure.Log.NLog.Tests
{
    [TestFixture()]
    public class NLoggerTests
    {
        [Test()]
        public void FatalTest()
        {
            ILogger nLogger = Substitute.For<ILogger>();
            NLogger logger = new NLogger(nLogger);

            logger.Fatal("message", new object[] { });

            nLogger.Received().Fatal("message", Arg.Any<object[]>());
        }

        [Test()]
        public void ErrorTest()
        {
            ILogger nLogger = Substitute.For<ILogger>();
            NLogger logger = new NLogger(nLogger);

            logger.Error("message", new object[] { });

            nLogger.Received().Error("message", Arg.Any<object[]>());
        }

        [Test()]
        public void WarnTest()
        {
            ILogger nLogger = Substitute.For<ILogger>();
            NLogger logger = new NLogger(nLogger);

            logger.Warn("message", new object[] { });

            nLogger.Received().Warn("message", Arg.Any<object[]>());
        }

        [Test()]
        public void InfoTest()
        {
            ILogger nLogger = Substitute.For<ILogger>();
            NLogger logger = new NLogger(nLogger);

            logger.Info("message", new object[] { });

            nLogger.Received().Info("message", Arg.Any<object[]>());
        }

        [Test()]
        public void DebugTest()
        {
            ILogger nLogger = Substitute.For<ILogger>();
            NLogger logger = new NLogger(nLogger);

            logger.Debug("message", new object[] { });

            nLogger.Received().Debug("message", Arg.Any<object[]>());
        }

        [Test()]
        public void TraceTest()
        {
            ILogger nLogger = Substitute.For<ILogger>();
            NLogger logger = new NLogger(nLogger);

            logger.Trace("message", new object[] { });

            nLogger.Received().Trace("message", Arg.Any<object[]>());
        }
    }
}