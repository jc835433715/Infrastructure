﻿using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mapper.AutoMapper.Tests
{
    class AddressMapperConfig : MapperConfigBase
    {
        public override void Load(MapperConfigurationExpression config)
        {
            config.CreateMap<Address, AddressDto>();
        }
    }

    public class Address
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string PostCode { get; set; }
    }

    public class AddressDto
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string PostCode { get; set; }
    }
}
