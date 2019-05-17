using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;

namespace Infrastructure.UT
{
    [TestFixture]
    class LiveUnitTestingTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Console.WriteLine("OneTimeSetUp");
        }

        [SetUp]
        public void SetUp()
        {
            Console.WriteLine("SetUp");
        }

        [Test]
        public void Test()
        {
            Console.WriteLine("Test");
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("TearDown");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Console.WriteLine("OneTimeTearDown");
        }
    }
}
