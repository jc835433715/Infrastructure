using CsvHelper.Configuration;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace Infrastructure.Csv.CsvHelper.Tests
{
    [TestFixture()]
    public class CsvHelperImpTests
    {
        public class Record
        {
            public DateTime Time { get; set; }

            public string Message { get; set; }
        }

        public class RecordMap : ClassMap<Record>
        {
            public RecordMap()
            {
                Map(m => m.Time).Name("Time").TypeConverterOption.Format("HH:mm:ss");
                Map(m => m.Message).Name("Message,Head");
            }
        }

        private DateTime dt;

        [Test()]
        public void ReadTest()
        {
            var csv = new CsvHelperImp<Record, RecordMap>();
            var record = new Record();

            dt = DateTime.Now;

            csv.Write($@"{AppDomain.CurrentDomain.BaseDirectory }\Csv1\T.csv", new Record[] { new Record() { Time = dt, Message = "\"Go,Go,Go!\"" } });
            record = csv.Read($@"{AppDomain.CurrentDomain.BaseDirectory }\Csv1\T.csv").Single();

            Assert.IsTrue((dt - record.Time) < TimeSpan.FromSeconds(1));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Directory.Delete($@"{AppDomain.CurrentDomain.BaseDirectory }\Csv1", true);
        }
    }
}