using Infrastructure.ComPort.Imp.Net;
using Infrastructure.ComPort.Interface;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Infrastructure.BarcodeReader.Imp.Tests
{
    [TestFixture()]
    public class NetBarcodeReaderTests
    {
        [Test()]
        public void Test()
        {
            Regex regex = new Regex("^[A-Za-z0-9]+$");

            Assert.IsTrue(regex.IsMatch("123"));
            Assert.IsTrue(regex.IsMatch("abc"));
            Assert.IsTrue(regex.IsMatch("abc123"));
            Assert.IsTrue(regex.IsMatch("Abc123"));
            Assert.IsTrue(regex.IsMatch("ABC123"));

            Assert.IsFalse(regex.IsMatch(""));
            Assert.IsFalse(regex.IsMatch(" "));
            Assert.IsFalse(regex.IsMatch("?"));
            Assert.IsFalse(regex.IsMatch("? "));
            Assert.IsFalse(regex.IsMatch(" 123"));
            Assert.IsFalse(regex.IsMatch("?123"));
            Assert.IsFalse(regex.IsMatch("123\r\n"));
            Assert.IsFalse(regex.IsMatch("#$123"));
        }

        [Test()]
        public void ReverseTest()
        {
            var list = new List<byte>() { 1, 2 };
            var listReverse = list.ToArray().Reverse().ToList();


            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, listReverse[0]);
        }
    }
}