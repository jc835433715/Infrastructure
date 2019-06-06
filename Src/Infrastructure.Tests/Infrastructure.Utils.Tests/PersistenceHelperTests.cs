using Infrastructure.Serialization.Factory;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Infrastructure.Utils.Tests
{
    [TestFixture()]
    [Serializable]
    public class PersistenceHelperTests
    {
        public PersistenceHelperTests()
        {
            Pa = string.Empty;
            Pb = new List<string>();
            Pc = new List<string>();
        }

        public string Pa { get; set; }

        public List<string> Pb { get; set; }

        private List<string> Pc;

        [Test()]
        public void Test()
        {
            var value = new PersistenceHelperTests()
            {
                Pa = "Pa",
                Pb = new List<string>()
                {
                    "Pb"
                },
                Pc = new List<string>()
                {
                    "Pc"
                }
            };
            PersistenceHelperTests obj = null;

            Infrastructure.Serialization.Factory.PersistenceHelper.SaveBin($@"{AppDomain.CurrentDomain.BaseDirectory }\T.bin", value);

            obj = PersistenceHelper.RecoveryBin<PersistenceHelperTests>($@"{AppDomain.CurrentDomain.BaseDirectory }\T.bin");

            Assert.AreEqual(value.Pa, obj.Pa);
            Assert.IsFalse(value.Pb.Except(obj.Pb).Any());
            Assert.IsFalse(value.Pc.Except(obj.Pc).Any());
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            File.Delete($@"{AppDomain.CurrentDomain.BaseDirectory }\T.bin");
        }
    }
}

