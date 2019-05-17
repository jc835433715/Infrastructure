using NUnit.Framework;
using Infrastructure.Serialization.YamlDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Serialization.YamlDotNet.Tests
{
    [TestFixture()]
    public class YamlConverterTests
    {

        public class Order
        {
            public string Receipt { get; set; }

            public DateTime Date { get; set; }

            public Customer Customer { get; set; }

            public List<OrderItem> Items { get; set; }

            public Address BillTo { get; set; }

            public Address ShipTo { get; set; }

            public string SpecialDelivery { get; set; }
        }

        public class Customer
        {
            public string Given { get; set; }

            public string Family { get; set; }
        }

        public class OrderItem
        {
            public string PartNo { get; set; }

            public string Descrip { get; set; }

            public decimal Price { get; set; }

            public int Quantity { get; set; }
        }

        public class Address
        {
            public string Street { get; set; }

            public string City { get; set; }

            public string State { get; set; }
        }


        [Test()]
        public void DeserializeTest()
        {
            YamlConverter yamlConverter = new YamlConverter();
            var order = yamlConverter.Deserialize<Order>(document);

            Assert.IsNotNull(order);
        }

        [Test()]
        public void DeserializeTest1()
        {
            YamlConverter yamlConverter = new YamlConverter();
            var addrss = new Address()
            {
                City = "City",
                State = "State",
                Street = "Street"
            };
            var value = yamlConverter.Serialize(addrss);
            var actual = yamlConverter.Deserialize(value, typeof(Address));
            
            Assert.AreEqual("City", (actual as Address).City);
        }

        [Test()]
        public void SerializeTest()
        {
            YamlConverter yamlConverter = new YamlConverter();
            var addrss = new Address()
            {
                City = "City",
                State = "State",
                Street = "Street"
            };
            var value = yamlConverter.Serialize(addrss);

            Assert.AreEqual("street: Street\r\ncity: City\r\nstate: State\r\n", value);
        }

        private const string document = @"---
            receipt:    Oz-Ware Purchase Invoice
            date:        2007-08-06
            customer:
                given:   Dorothy
                family:  Gale

            items:
                - partNo:   A4786
                  descrip:   Water Bucket (Filled)
                  price:     1.47
                  quantity:  4

                - partNo:   E1628
                  descrip:   High Heeled ""Ruby"" Slippers
                  price:     100.27
                  quantity:  1

            billTo:  &id001
                street: |-
                        123 Tornado Alley
                        Suite 16
                city:   East Westville
                state:  KS

            shipTo:  *id001

            specialDelivery: >
                Follow the Yellow Brick
                Road to the Emerald City.
                Pay no attention to the
                man behind the curtain.
...";
    }
}