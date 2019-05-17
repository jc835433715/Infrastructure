using Infrastructure.Config.Interface;
using Infrastructure.Config.Yaml;

namespace Infrastructure.Config.Factory
{
    /// <summary>
    /// 配置工厂
    /// </summary>
    public static class ConfigFactory
    {
        /// <summary>
        /// 参加配置对象
        /// </summary>
        /// <typeparam name="TConfigInfo">配置信息</typeparam>
        /// <returns>配置对象</returns>
        public static IConfig<TConfigInfo> Create<TConfigInfo>()
            where TConfigInfo : class, new()
        {
            return new ConfigYaml<TConfigInfo>();
        }
    }
}
