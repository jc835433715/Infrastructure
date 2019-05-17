using Infrastructure.Plc.Interface;
using Infrastructure.PlcMonitor.Interface;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Plc.Factory.Tests
{
    [TestFixture()]
    public class PlcAddressHelperTests
    {
        [Test()]
        public void SortPlcAddressForUTTest00()
        {
            var eventInfoes = new List<PlcAddress>
            {
                new PlcAddress(){ Value= "DM003" },
                new PlcAddress(){ Value= "DM001" },
                new PlcAddress(){ Value= "DM002" },
                new PlcAddress(){ Value= "DM005" },
                new PlcAddress(){ Value= "DM007" },
                new PlcAddress(){ Value= "DM006" },
                new PlcAddress(){ Value= "DM009" },
                new PlcAddress(){ Value= "E0_123"},
            };
            var result = PlcAddressSortHelper.Sort<short>(eventInfoes);

            Assert.AreEqual(2, result.AddressSegments.Count());
            Assert.IsTrue(result.AddressSegments.Any(e => e.StartAddress.Value == "DM001"));
            Assert.IsTrue(result.AddressSegments.Any(e => e.StartAddress.Value == "DM005"));

            Assert.AreEqual(2, result.AddressNotSegment.Count());
            Assert.IsTrue(result.AddressNotSegment.Addresses.Any(e => e.Value == "DM009"));
            Assert.IsTrue(result.AddressNotSegment.Addresses.Any(e => e.Value == "E0_123"));
        }

        [Test()]
        public void SortPlcAddressForUTTest01()
        {
            var eventInfoes = new List<PlcAddress>
            {
                new PlcAddress(){ Value= "DM004" },
                new PlcAddress() { Value = "DM000" },
                new PlcAddress() { Value = "DM002" },
                new PlcAddress() { Value = "DM008" },
                new PlcAddress() { Value = "DM010" },
                new PlcAddress() { Value = "DM018" }
            };
            var result = PlcAddressSortHelper.Sort<int>(eventInfoes);

            Assert.AreEqual(2, result.AddressSegments.Count());
            Assert.IsTrue(result.AddressSegments.Any(e => e.StartAddress.Value == "DM000"));
            Assert.IsTrue(result.AddressSegments.Any(e => e.StartAddress.Value == "DM008"));

            Assert.AreEqual(1, result.AddressNotSegment.Count());
            Assert.IsTrue(result.AddressNotSegment.Addresses.Any(e => e.Value == "DM018"));
        }

        [Test()]
        public void SortPlcAddressForUTTes02()
        {
            var eventInfoes = new List<PlcAddress>
            {
                new PlcAddress() { Value = "DM001" }
            };

            var result = PlcAddressSortHelper.Sort<short>(eventInfoes);

            Assert.AreEqual(0, result.AddressSegments.Count());

            Assert.AreEqual(1, result.AddressNotSegment.Count());
            Assert.IsTrue(result.AddressNotSegment.Addresses.Any(e => e.Value == "DM001"));
        }

        [Test()]
        public void SortPlcAddressForUTTes03()
        {
            var eventInfoes = new List<PlcAddress>
            {
                new PlcAddress() { Value = "DM001" },
                new PlcAddress() { Value = "DM003" }
            };

            var result = PlcAddressSortHelper.Sort<short>(eventInfoes);

            Assert.AreEqual(0, result.AddressSegments.Count());

            Assert.AreEqual(2, result.AddressNotSegment.Count());
            Assert.IsTrue(result.AddressNotSegment.Addresses.Any(e => e.Value == "DM001"));
            Assert.IsTrue(result.AddressNotSegment.Addresses.Any(e => e.Value == "DM003"));
        }
        
        [Test()]
        public void SortPlcAddressForUTTes04()
        {
            var eventInfoes = new List<PlcAddress>
            {
                new PlcAddress() { Value = "DM001" },
                new PlcAddress() { Value = "DM003" },
                new PlcAddress() { Value = "DM005" },
                new PlcAddress() { Value = "DM006" }
            };

            var result = PlcAddressSortHelper.Sort<short>(eventInfoes);

            Assert.AreEqual(1, result.AddressSegments.Count());
            Assert.IsTrue(result.AddressSegments.Any(e => e.StartAddress.Value == "DM005"));

            Assert.AreEqual(2, result.AddressNotSegment.Count());
            Assert.IsTrue(result.AddressNotSegment.Addresses.Any(e => e.Value == "DM001"));
            Assert.IsTrue(result.AddressNotSegment.Addresses.Any(e => e.Value == "DM003"));
        }
    }
}