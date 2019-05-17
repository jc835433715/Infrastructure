using Infrastructure.Ioc.Interface;
using Infrastructure.Ioc.Ninject;

namespace Infrastructure.Ioc.Factory
{
    /// <summary>
    /// 依赖注入解析器管理者
    /// </summary>
    public static class DependencyResolverManager
    {
        static DependencyResolverManager()
        {
            dependencyResolver = new NinjectDependencyResolver();
        }

        /// <summary>
        /// 设置Ninject的kernel
        /// </summary>
        /// <param name="kernel">kernel</param>
        public static void SetKernel(object kernel)
        {
            dependencyResolver = new NinjectDependencyResolver(kernel);
        }

        /// <summary>
        /// 设置依赖注入解析器
        /// </summary>
        /// <param name="dependencyResolver"></param>
        public static void SetDependencyResolver(IDependencyResolver dependencyResolver)
        {
            DependencyResolverManager.dependencyResolver = dependencyResolver;
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
