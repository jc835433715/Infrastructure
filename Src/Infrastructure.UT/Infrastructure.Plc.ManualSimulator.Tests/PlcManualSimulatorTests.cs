using NUnit.Framework;
using Infrastructure.Plc.ManualSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Infrastructure.Plc.ManualSimulator.Tests
{
    [TestFixture()]
    public class PlcManualSimulatorTests
    {
        [Test()]
        public void InitializeTest()
        {
            var plc = new PlcManualSimulator();

            //plc.Initialize();

            //plc.Read<short>(new Interface.Address()
            //{
            //    Name = "心跳",
            //    Offset = 0,
            //    Type = Interface.AddressType.Short,
            //    Value = "EM0"
            //});

            //Thread.Sleep(-1);
        }
    }
}