using Infrastructure.Ioc.Interface;
using NUnit.Framework;
using System;
using System.Linq;

namespace Infrastructure.Ioc.Ninject.Tests
{
    [TestFixture()]
    public class NinjectDependencyResolverTests
    {
        private IDependencyResolver dependencyResolver;


        [Test, Order(0)]
        public void InitializeForUT()
        {
            this.dependencyResolver = new NinjectDependencyResolver(new DependencyConfig(), null);
        }

        [Test]
        public void InitializeTest()
        {
            var dependencyResolver = new NinjectDependencyResolver(new string[] { "Infrastructure.UT" });
            ILogger logger = null;

            logger = dependencyResolver.GetService<ILogger>(LoggerType.File.ToString());

            Assert.IsInstanceOf<FileLogger>(logger);
        }

        [Test()]
        public void GetServiceGenericTest()
        {
            var logger = dependencyResolver.GetService<ILogger>(LoggerType.File.ToString());

            Assert.IsInstanceOf<FileLogger>(logger);
        }

        [Test()]
        public void GetServicesGenericTest()
        {
            var loggers = dependencyResolver.GetServices<ILogger>();

            Assert.IsTrue(loggers.Count() == 2);
        }

        [Test()]
        public void GetServiceTest()
        {
            var logger = dependencyResolver.GetService(typeof(ILogger), LoggerType.File.ToString());

            Assert.IsInstanceOf<FileLogger>(logger);
        }

        [Test()]
        public void GetServicesTest()
        {
            var loggers = dependencyResolver.GetServices(typeof(ILogger));

            Assert.IsTrue(loggers.Count() == 2);
        }

        [Test()]
        public void GetLazyServiceTest()
        {
            var logger = dependencyResolver.GetLazyService<ILogger>(LoggerType.File.ToString());

            Assert.IsInstanceOf<FileLogger>(logger.Value);
        }

        [Test()]
        public void GetLazyServicesTest()
        {
            var loggers = dependencyResolver.GetLazyServices<ILogger>();

            Assert.IsTrue(loggers.Value.Count() == 2);
        }

        [Test]
        public void MultipleBindingsTest()
        {
            IA a = dependencyResolver.GetService<IA>();
            IB b = dependencyResolver.GetService<IB>();

            Assert.AreSame(a, b);
        }
    }
}