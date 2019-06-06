using NUnit.Framework;
using Infrastructure.Ioc.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Ioc.Interface;

namespace Infrastructure.Ioc.Autofac.Tests
{
    [TestFixture()]
    public class AutofacDependencyResolverTests
    {
        private IDependencyResolver dependencyResolver;

        [Test, Order(0)]
        public void InitializeForUT()
        {
            this.dependencyResolver = new AutofacDependencyResolver(new DependencyConfig());
        }

        [Test]
        public void InitializeTest()
        {
            var dependencyResolver = new AutofacDependencyResolver(new string[] { "Infrastructure.Tests" });
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
        public void GetServiceTest()
        {
            var logger = dependencyResolver.GetService(typeof(ILogger), LoggerType.File.ToString());

            Assert.IsInstanceOf<FileLogger>(logger);
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