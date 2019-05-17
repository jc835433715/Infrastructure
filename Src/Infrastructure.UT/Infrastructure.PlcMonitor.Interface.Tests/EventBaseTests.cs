using NUnit.Framework;
using Infrastructure.PlcMonitor.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Plc.Interface;

namespace Infrastructure.PlcMonitor.Interface.Tests
{
    [TestFixture()]
    public class EventBaseTests
    {
        [Test()]
        public void CloneTest()
        {
            EndValueReadEvent<int> endValueReadEvent = new EndValueReadEvent<int>()
            {
                PlcAddress = PlcAddress.Empty
            };
            var endValueReadEventCloned = endValueReadEvent.Clone();

            Assert.IsInstanceOf<IEvent>(endValueReadEventCloned);
            Assert.IsInstanceOf<EndValueReadEvent<int>>(endValueReadEventCloned);
            Assert.AreEqual(endValueReadEvent, (endValueReadEventCloned as EndValueReadEvent<int>));
            Assert.AreEqual(endValueReadEvent.PlcAddress, (endValueReadEventCloned as EndValueReadEvent<int>).PlcAddress);
        }
    }
}