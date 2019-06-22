using NUnit.Framework;
using Infrastructure.Log.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Log.Factory.Tests
{
    [TestFixture()]
    public class LogManagerTests
    {
        [Test()]
        public void ClearTest()
        {
            LogManager.Clear(@".\",0);

            LogManager.Clear(string.Empty, 0);
        }
    }
}