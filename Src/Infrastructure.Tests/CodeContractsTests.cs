using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UT
{
    [TestFixture]
    class CodeContractsTests
    {
        [Test]
        public void Test()
        {
            Add(0, 0);
        }


        private int Add(int a, int b)
        {
            Contract.Requires(a > 0);
            Contract.Requires(b > 0);
            Contract.Ensures(a + b > 0);

            return a + b;
        }
    }
}
