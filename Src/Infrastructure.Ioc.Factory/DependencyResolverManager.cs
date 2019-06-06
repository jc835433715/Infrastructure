using Infrastructure.Ioc.Autofac;
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
        /// 初始化
        /// </summary>
        /// <param name="dependencyConfigModuleAssemblyStrings">依赖注入配置模块程序集</param>
        /// <param name="type">Ioc类型</param>
        /// <param name="dependencyConfigModuleNameSpaceStrings">依赖注入配置模块程序集命名空间</param>
        public static void Initialize(IEnumerable<string> dependencyConfigModuleAssemblyStrings, IocType type = IocType.Ninject, IEnumerable<string> dependencyConfigModuleNameSpaceStrings = null)
        {
            if (type == IocType.Ninject)
            {
                dependencyResolver = new NinjectDependencyResolver(dependencyConfigModuleAssemblyStrings, dependencyConfigModuleNameSpaceStrings);
            }

            if (type == IocType.Autofac)
            {
                dependencyResolver = new AutofacDependencyResolver(dependencyConfigModuleAssemblyStrings, dependencyConfigModuleNameSpaceStrings);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dependencyConfigModule">依赖注入配置模块</param>
        /// <param name="type">Ioc类型</param>
        /// <param name="kernel">Ninject的kernel</param>
        public static void Initialize(object dependencyConfigModule, IocType type = IocType.Ninject, object kernel = null)
        {
            if (type == IocType.Ninject)
            {
                dependencyResolver = new NinjectDependencyResolver(dependencyConfigModule, kernel);
            }

            if (type == IocType.Autofac)
            {
                dependencyResolver = new AutofacDependencyResolver(dependencyConfigModule, kernel);
            }
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

    /// <summary>
    /// Ioc类型
    /// </summary>
    public enum IocType
    {
        /// <summary>
        /// Autofac
        /// </summary>
        Autofac,
        /// <summary>
        /// Ninject
        /// </summary>
        Ninject
    }
}
