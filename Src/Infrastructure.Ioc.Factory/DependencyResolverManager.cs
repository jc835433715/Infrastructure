using Infrastructure.Ioc.Interface;
using Infrastructure.Ioc.Ninject;
using System.Collections.Generic;

namespace Infrastructure.Ioc.Factory
{
    /// <summary>
    /// 依赖注入解析器管理者
    /// </summary>
    public static class DependencyResolverManager
    {
        /// <summary>
        /// 初始化Ninject
        /// </summary>
        /// <param name="dependencyConfigAssemblyStrings">依赖注入配置程序集</param>
        /// <param name="dependencyConfigNameSpaceStrings">依赖注入配置程序集命名空间</param>
        /// <param name="kernel">Ninject的kernel</param>
        public static void Initialize(IEnumerable<string> dependencyConfigAssemblyStrings, IEnumerable<string> dependencyConfigNameSpaceStrings = null, object kernel = null)
        {
            dependencyResolver = new NinjectDependencyResolver(dependencyConfigAssemblyStrings, dependencyConfigNameSpaceStrings, kernel);
        }

        /// <summary>
        /// 初始化Ninject
        /// <param name="dependencyConfig">继承DependencyConfigBase的对象</param>
        /// <param name="kernel">Ninject的kernel</param>
        /// </summary>
        public static void Initialize(object dependencyConfig, object kernel = null)
        {
            dependencyResolver = new NinjectDependencyResolver(dependencyConfig, kernel);
        }
        
        /// <summary>
        /// 获取依赖注入解析器
        /// </summary>
        /// <returns>依赖注入解析器</returns>
        public static IDependencyResolver GetDependencyResolver()
        {
            return dependencyResolver;
        }

        private static IDependencyResolver dependencyResolver;
    }
}
