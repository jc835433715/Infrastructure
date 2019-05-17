using AutoMapper.Configuration;
using System;
using AM = AutoMapper;

namespace Infrastructure.Mapper.AutoMapper
{
    public class AutoMapperImp : Infrastructure.Mapper.Interface.IMapper
    {
        public AutoMapperImp(IMapperConfigFactory mapperConfigFactory, bool isAssertConfigurationIsValid = false)
        {
            this.cfg = new MapperConfigurationExpression();

            mapperConfigFactory.Create().ForEach(e => e.Load(cfg));

            AM.Mapper.Initialize(cfg);
            CheckConfigurationIsValid(isAssertConfigurationIsValid);
        }

        public AutoMapperImp(object mapperConfig, bool isAssertConfigurationIsValid = false)
        {
            this.cfg = new MapperConfigurationExpression();

            (mapperConfig as MapperConfigBase).Load(cfg);

            AM.Mapper.Initialize(cfg);
            CheckConfigurationIsValid(isAssertConfigurationIsValid);
        }


        public TDestination Map<TDestination>(object source)
        {
            return AM.Mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return AM.Mapper.Map<TSource, TDestination>(source);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return AM.Mapper.Map(source, destination, sourceType, destinationType);
        }

        public void Reset()
        {
            AM.Mapper.Reset();
        }

        private void CheckConfigurationIsValid(bool isAssertConfigurationIsValid)
        {
            if (isAssertConfigurationIsValid)
            {
                AM.Mapper.AssertConfigurationIsValid();
            }
        }

        private MapperConfigurationExpression cfg;
    }
}
