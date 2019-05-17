using Infrastructure.Serialization.Interface;
using NUnit.Framework;
using System.Dynamic;

namespace Infrastructure.Serialization.Newtonsoft.Tests
{
    [TestFixture()]
    public class JsonConverterTests
    {
        private class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }

        private IConverter converter;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.converter = new JsonConverter();
        }

        [Test()]
        public void SerializeTest()
        {
            dynamic person = new ExpandoObject();
            string actual = string.Empty;

            person.Name = "YHY";
            person.Age = 1;

            actual = converter.Serialize(person);

            Assert.AreEqual("{\"Name\":\"YHY\",\"Age\":1}", actual);
        }

        [Test()]
        public void DeserializeGenericTest()
        {
            Person person = converter.Deserialize<Person>("{\"Name\":\"YHY\",\"Age\":1}");

            Assert.AreEqual("YHY", person.Name);
            Assert.AreEqual(1, person.Age);
        }

        [Test()]
        public void DeserializeTest()
        {
            dynamic person = converter.Deserialize("{\"Name\":\"YHY\",\"Age\":1}", typeof(ExpandoObject));

            Assert.AreEqual("YHY", person.Name);
            Assert.AreEqual(1, person.Age);
        }
    }
}