using System.Collections.Generic;

namespace Infrastructure.Mapper.AutoMapper
{
    public interface IMapperConfigFactory
    {
        List<MapperConfigBase> Create();
    }
}