using System;
using System.Collections.Generic;

namespace Infrastructure.Ioc.Interface
{
    /// <summary>
    /// 依赖注入解析器
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dependencyConfigAssemblyStrings">依赖注入配置程序集的长格式</param>
        void Initialize(IEnumerable<string> dependencyConfigAssemblyStrings);

        /// <summary>
        /// 初始化
        /// <param name="dependencyConfig">继承DependencyConfigBase的对象</param>
        /// </summary>
        void InitializeForUT(object dependencyConfig);

        /// <summary>
        /// 获取服务对象
        /// </summary>
        /// <typeparam name="T">服务对象类型</typeparam>
        ///<param name="name">绑定名</param>
        /// <returns>服务对象</returns>
        T GetService<T>(string name = null);

        /// <summary>
        /// 获取服务对象
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="name">绑定名</param>
        /// <returns>服务对象</returns>
        object GetService(Type serviceType, string name = null);

        /// <summary>
        /// 获取服务对象枚举数
        /// </summary>
        /// <typeparam name="T">服务对象类型</typeparam>
        /// <param name="name">绑定名</param>
        /// <returns>服务对象枚举数</returns>
        IEnumerable<T> GetServices<T>(string name = null);

        /// <summary>
        ///  获取服务对象枚举数
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <param name="name">绑定名</param>
        /// <returns>服务对象枚举数</returns>
        IEnumerable<object> GetServices(Type serviceType, string name = null);

        /// <summary>
        /// 获取延迟加载服务对象
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="name">绑定名</param>
        /// <returns>延迟加载服务对象</returns>
        Lazy<T> GetLazyService<T>(string name = null);

        /// <summary>
        /// 获取延迟加载服务对象枚举数
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="name">绑定名</param>
        /// <returns>延迟加载服务对象枚举数</returns>
        Lazy<IEnumerable<T>> GetLazyServices<T>(string name = null);
    }
}
