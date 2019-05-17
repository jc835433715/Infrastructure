using AutoMapper.Configuration;

namespace Infrastructure.Mapper.AutoMapper
{
    public abstract class MapperConfigBase
    {
        public abstract void Load(MapperConfigurationExpression config);
    }
}
