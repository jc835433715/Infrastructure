using NUnit.Framework;
using System;
using System.IO;

namespace Infrastructure.Csv.Imp.Tests
{
    [TestFixture()]
    public class CsvImpTests
    {
        [Test()]
        public void WriteTest()
        {
            var csv = new CsvImp();

            csv.Write($@"{AppDomain.CurrentDomain.BaseDirectory }\Csv2\T.csv", new string[] { "Time", "Message,Head" }, new string[] { "Money", "\"Go,Go,Go!\"" });
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Directory.Delete($@"{AppDomain.CurrentDomain.BaseDirectory }\Csv2", true);
        }
    }
}