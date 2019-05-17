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

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.dependencyResolver = new NinjectDependencyResolver();
        }
        [Test, Order(0)]
        public void InitializeForUT()
        {
            dependencyResolver.InitializeForUT(new DependencyConfig());
        }

        [Test]
        public void InitializeTest()
        {
            using (var dependencyResolver = new NinjectDependencyResolver())
            {
                ILogger logger = null;

                dependencyResolver.Initialize(new string[] { "Infrastructure.UT" });
                logger = dependencyResolver.GetService<ILogger>(LoggerType.File.ToString());

                Assert.IsInstanceOf<FileLogger>(logger);
            }
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

        [OneTimeTearDown]
        public void DisposeTest()
        {
            (this.dependencyResolver as IDisposable).Dispose();
        }
    }
}