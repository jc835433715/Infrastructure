using System;
using System.Collections.Generic;

namespace Infrastructure.Mapper.Interface
{
    /// <summary>
    /// 映射器接口
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// 将源对象映射成目标对象
        /// </summary>
        /// <typeparam name="TDestination">目标对象类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>目标对象</returns>
        TDestination Map<TDestination>(object source);

        /// <summary>
        /// 将源对象映射成目标对象
        /// </summary>
        /// <typeparam name="TSource">源对象类型</typeparam>
        /// <typeparam name="TDestination">目标对象类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>目标对象</returns>
        TDestination Map<TSource, TDestination>(TSource source);

        /// <summary>
        ///  将源对象映射成目标对象
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="destination">目的对象</param>
        /// <param name="sourceType">源对象类型</param>
        /// <param name="destinationType">目的对象类型</param>
        /// <returns>目标对象</returns>
        object Map(object source, object destination, Type sourceType, Type destinationType);

        /// <summary>
        /// 重置映射配置
        /// </summary>
        void Reset();
    }
}
