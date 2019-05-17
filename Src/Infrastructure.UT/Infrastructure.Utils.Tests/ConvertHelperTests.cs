using Infrastructure.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils.Tests
{
    [TestFixture]
    public class ConvertHelperTests
    {
        [Test]
        public void Test()
        {
            var value = new byte[] { 0x0, 0x00, 0x1, 0x01, 0xf, 0x0f };
            var hexString = ConvertHelper.ToString(value);
            var bytes = ConvertHelper.ToBytes(hexString);

            Assert.AreEqual("00 00 01 01 0F 0F", hexString);
            Assert.IsFalse(value.Except(bytes).Any() && bytes.Except(value).Any());
        }
    }
}
