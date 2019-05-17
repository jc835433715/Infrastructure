using NUnit.Framework;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils.Tests
{
    [TestFixture()]
    public class StopwatchWrapperTests
    {
        [Test()]
        public void Test()
        {
            using (new StopwatchWrapper("休眠"))
            {
                ThreadHelper.Sleep(100);
            };
        }

        [Test()]
        public void StopwatchWrapperTest()
        {
            uint x = uint.MaxValue;

            x = ++x;
        }
    }
}