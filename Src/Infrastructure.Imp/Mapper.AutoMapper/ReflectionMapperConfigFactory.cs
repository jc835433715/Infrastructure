using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Mapper.AutoMapper
{
    public class ReflectionMapperConfigFactory : IMapperConfigFactory
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mapperConfigAssemblyStrings">映射配置程序集名称的长格式</param>
        public ReflectionMapperConfigFactory(IEnumerable<string> mapperConfigAssemblyStrings)
        {
            this.mapperConfigAssemblyStrings = mapperConfigAssemblyStrings;
        }

        public List<MapperConfigBase> Create()
        {
            var result = new List<MapperConfigBase>();

            mapperConfigAssemblyStrings.ToList().ForEach(e => result.AddRange(GetAllMapperConfig(e)));

            return result;
        }

        private List<MapperConfigBase> GetAllMapperConfig(string mapperConfigAssemblyString)
        {
            var mapperConfigAssembly = Assembly.Load(mapperConfigAssemblyString);

            return (from t in mapperConfigAssembly.GetTypes()
                    where t.BaseType == typeof(MapperConfigBase)
                    select mapperConfigAssembly.CreateInstance(t.FullName) as MapperConfigBase).ToList();
        }

        private readonly IEnumerable<string> mapperConfigAssemblyStrings;
    }
}
