using Infrastructure.Ioc.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Mapper.AutoMapper
{
    public class IocMapperConfigFactory : IMapperConfigFactory
    {
        public IocMapperConfigFactory(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        public List<MapperConfigBase> Create()
        {
            return dependencyResolver.GetServices<MapperConfigBase>().ToList();
        }

        private readonly IDependencyResolver dependencyResolver;
    }
}
