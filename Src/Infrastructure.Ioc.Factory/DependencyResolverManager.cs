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
        /// <param name="dependencyConfigModuleAssemblyStrings">依赖注入配置模块程序集</param>
        /// <param name="dependencyConfigModuleNameSpaceStrings">依赖注入配置模块程序集命名空间</param>
        public static void Initialize(IEnumerable<string> dependencyConfigModuleAssemblyStrings, IEnumerable<string> dependencyConfigModuleNameSpaceStrings = null)
        {
            dependencyResolver = new NinjectDependencyResolver(dependencyConfigModuleAssemblyStrings, dependencyConfigModuleNameSpaceStrings);
        }

        /// <summary>
        /// 初始化Ninject
        /// <param name="dependencyConfigModule">依赖注入配置模块</param>
        /// <param name="kernel">Ninject的kernel</param>
        /// </summary>
        public static void Initialize(object dependencyConfigModule, object kernel = null)
        {
            dependencyResolver = new NinjectDependencyResolver(dependencyConfigModule, kernel);
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
