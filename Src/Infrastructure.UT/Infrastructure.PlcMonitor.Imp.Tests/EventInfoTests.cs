using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UT.Infrastructure.PlcMonitor.Imp.Tests
{
    [TestFixture]
    public class EventInfoTests
    {
        [Test]
        public void ActionTest()
        {
            bool invoked = false;
            Action<dynamic> action = e => invoked = true;

            action(new object());

            Assert.IsTrue(invoked);
        }
    }
}
