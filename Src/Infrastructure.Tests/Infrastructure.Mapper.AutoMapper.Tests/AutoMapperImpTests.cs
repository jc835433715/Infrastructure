using NUnit.Framework;
using Infrastructure.Mapper.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Mapper.Interface;

namespace Infrastructure.Mapper.AutoMapper.Tests
{
    /// <summary>
    /// 参考示例：
    /// http://www.cnblogs.com/jobs2/p/3503990.html
    /// </summary>
    [TestFixture()]
    public class AutoMapperImpTests
    {
        private IMapper mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.mapper = new AutoMapperImp(new ReflectionMapperConfigFactory(new string[] { "Infrastructure.Tests" }), true);
        }

        [Test(), Ignore("单进程内只能初始化一次")]
        public void InitializeForUTTest()
        {
            IMapper mapper = new AutoMapperImp(new AddressMapperConfig(), true);
        }

        [Test()]
        public void MapTest()
        {
            Address address = new Address()
            {
                Country = "中国",
                City = "深圳",
                Street = "龙华街道办",
                PostCode = "518000"
            };
            AddressDto addressDto = mapper.Map<AddressDto>(address);

            Assert.AreEqual(address.Country, addressDto.Country);
            Assert.AreEqual(address.City, addressDto.City);
            Assert.AreEqual(address.Street, addressDto.Street);
            Assert.AreEqual(address.PostCode, addressDto.PostCode);
        }

        [Test()]
        public void MapTest1()
        {
            Address address = new Address()
            {
                Country = "中国",
                City = "深圳",
                Street = "龙华街道办",
                PostCode = "518000"
            };
            AddressDto addressDto = new AddressDto();

            mapper.Map(address, addressDto, typeof(Address), typeof(AddressDto));

            Assert.AreEqual(address.Country, addressDto.Country);
            Assert.AreEqual(address.City, addressDto.City);
            Assert.AreEqual(address.Street, addressDto.Street);
            Assert.AreEqual(address.PostCode, addressDto.PostCode);
        }

        [Test()]
        public void MapTest2()
        {
            Address address = new Address()
            {
                Country = "中国",
                City = "深圳",
                Street = "龙华街道办",
                PostCode = "518000"
            };
            AddressDto addressDto = mapper.Map<Address, AddressDto>(address);

            Assert.AreEqual(address.Country, addressDto.Country);
            Assert.AreEqual(address.City, addressDto.City);
            Assert.AreEqual(address.Street, addressDto.Street);
            Assert.AreEqual(address.PostCode, addressDto.PostCode);
        }
    }
}