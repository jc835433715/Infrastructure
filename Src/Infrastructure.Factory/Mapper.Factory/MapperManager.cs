using Infrastructure.Ioc.Interface;
using Infrastructure.Mapper.AutoMapper;
using Infrastructure.Mapper.Interface;
using System.Collections.Generic;

namespace Infrastructure.Mapper.Factory
{
    /// <summary>
    /// 映射器管理者
    /// </summary>
    public static class MapperManager
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="mapperConfigAssemblyStrings">映射配置程序集名称的长格式</param>
        /// <param name="isAssertConfigurationIsValid">断言配置是否正确</param>
        public static void Initialize(IEnumerable<string> mapperConfigAssemblyStrings, bool isAssertConfigurationIsValid = false)
        {
            var mapperConfigFactory = new ReflectionMapperConfigFactory(mapperConfigAssemblyStrings);

            mapper = new AutoMapperImp(mapperConfigFactory, isAssertConfigurationIsValid);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dependencyResolver">依赖注入解析器</param>
        /// <param name="isAssertConfigurationIsValid">断言配置是否正确</param>
        public static void Initialize(IDependencyResolver dependencyResolver, bool isAssertConfigurationIsValid = false)
        {
            var mapperConfigFactory = new IocMapperConfigFactory(dependencyResolver);

            mapper = new AutoMapperImp(mapperConfigFactory, isAssertConfigurationIsValid);
        }

        /// <summary>
        /// 当前映射器
        /// </summary>
        public static IMapper CurrentMapper => mapper;

        /// <summary>
        /// 获取映射器
        /// </summary>
        /// <returns></returns>
        public static IMapper GetMapper()
        {
            return mapper;
        }

        private static IMapper mapper;
    }
}
