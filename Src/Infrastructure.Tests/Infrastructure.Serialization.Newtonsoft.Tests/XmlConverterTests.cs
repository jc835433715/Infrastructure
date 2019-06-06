using Infrastructure.Serialization.Interface;
using NUnit.Framework;

namespace Infrastructure.Serialization.Newtonsoft.Tests
{
    [TestFixture()]
    public class XmlConverterTests
    {
        class PersonXml
        {
            public class Value
            {
                public string Name { get; set; }

                public byte Age { get; set; }
            }

            public Value Person { get; set; }
        }

        private IConverter converter;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.converter = new XmlConverter();
        }
        [Test()]
        public void SerializeTest()
        {
            PersonXml value = new PersonXml
            {
                Person = new PersonXml.Value()
                {
                    Name = "YHY",
                    Age = 1
                }
            };
            string actual = string.Empty;

            actual = converter.Serialize(value);

            Assert.AreEqual("<Person><Name>YHY</Name><Age>1</Age></Person>", actual);
        }

        [Test()]
        public void DeserializeGenericTest()
        {
            PersonXml personXml = converter.Deserialize<PersonXml>("<Person><Name>YHY</Name><Age>1</Age></Person>");

            Assert.AreEqual("YHY", personXml.Person.Name);
            Assert.AreEqual(1, personXml.Person.Age);
        }

        [Test()]
        public void DeserializeTest()
        {
            PersonXml personXml = (PersonXml)converter.Deserialize("<Person><Name>YHY</Name><Age>1</Age></Person>", typeof(PersonXml));

            Assert.AreEqual("YHY", personXml.Person.Name);
            Assert.AreEqual(1, personXml.Person.Age);
        }
    }
}