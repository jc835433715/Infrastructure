using NUnit.Framework;
using Infrastructure.Plc.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Infrastructure.Plc.Interface;

namespace Infrastructure.Plc.Factory.Tests
{
    [TestFixture()]
    public class PlcAdapterTests
    {
        private PlcAdapter plcAdapter;
        private IPlc plc;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            plc = Substitute.For<IPlc>();
            plcAdapter = new PlcAdapter() { Plc = plc };
        }

        [Test()]
        public void ReadBoolTest()
        {
            var values = new string[] { };
            var errorInfo = string.Empty;
            var param = new string[] { "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1", "1" };

            plc.Read<ushort>(Arg.Any<PlcAddress>()).Returns(new ushort[] { 65535 });

            plcAdapter.ReadUshort("D1200", ref values, ref errorInfo);
            Assert.AreEqual(values, param);

            plcAdapter.ReadUshort("D", "1200", ref values, ref errorInfo);
            Assert.AreEqual(values, param);
        }

        [Test()]
        public void ReadShortTest()
        {
            var values = new int[] { };
            var errorInfo = string.Empty;
            var param = new int[] { 1, 2, 3, 4 };

            plc.Read<short>(Arg.Any<PlcAddress>()).Returns(new short[] { 1, 2, 3, 4 });

            plcAdapter.ReadShort("D1200", 4, ref values, ref errorInfo);
            Assert.AreEqual(values, param);

            plcAdapter.ReadShort("D", "1200", 4, ref values, ref errorInfo);
            Assert.AreEqual(values, param);
        }

        [Test()]
        public void ReadStringTest()
        {
            var values = new string[] { };
            var value = string.Empty;
            var errorInfo = string.Empty;
            var param = new string[] { "1", "2", "3", "4" };

            plc.Read<int>(Arg.Any<PlcAddress>()).Returns(new int[] { 2 });
            plc.Read<string>(Arg.Any<PlcAddress>()).Returns(new string[] { "1234" });

            plcAdapter.ReadString("D", "1200", 2, ref values, ref errorInfo);
            Assert.AreEqual(values, param);

            plcAdapter.ReadString("D1200", 4, ref value, ref errorInfo);
            Assert.AreEqual(values, param);

            plcAdapter.ReadStringByWord("D1200", "D1200", ref value, ref errorInfo);
            Assert.AreEqual(values, param);

            plcAdapter.ReadStringByBarcodeLength("D1202", "D1200", ref value, ref errorInfo);
            Assert.AreEqual(values, param);
        }

        [Test()]
        public void ReadIntTest()
        {
            var values = new int[] { };
            var errorInfo = string.Empty;
            var param = new int[] { 1, 2, 3, 4 };

            plc.Read<int>(Arg.Any<PlcAddress>()).Returns(new int[] { 1, 2, 3, 4 });

            plcAdapter.ReadInt("D1200", 4, ref values, ref errorInfo);
            Assert.AreEqual(values, param);

            plcAdapter.ReadInt("D", "1200", 4, ref values, ref errorInfo);
            Assert.AreEqual(values, param);
        }

        [Test()]
        public void ReadFloatTest()
        {
            var values = new float[] { };
            var errorInfo = string.Empty;
            var param = new float[] { 0.1f, 0.2f, 0.3f, 0.4f };

            plc.Read<float>(Arg.Any<PlcAddress>()).Returns(new float[] { 0.1f, 0.2f, 0.3f, 0.4f });

            plcAdapter.ReadFloat("D1200", 4, ref values, ref errorInfo);
            Assert.AreEqual(values, param);

            plcAdapter.ReadFloat("D", "1200", 4, ref values, ref errorInfo);
            Assert.AreEqual(values, param);
        }

    }
}