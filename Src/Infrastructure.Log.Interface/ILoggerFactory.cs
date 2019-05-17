using System;

namespace Infrastructure.Log.Interface
{
    /// <summary>
    /// 日志工厂
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// 根据类型获取日志对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>日志对象</returns>
        ILogger GetLogger<T>();

        /// <summary>
        /// 根据名称获取日志对象
        /// </summary>
        /// <param name="name">日志名称</param>
        /// <returns>日志对象</returns>
        ILogger GetLogger(string name);
    }
}
