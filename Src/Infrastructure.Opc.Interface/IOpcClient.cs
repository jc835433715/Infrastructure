using Infrastructure.Common.Interface;
using System;
using System.Collections.Generic;

namespace Infrastructure.Opc.Interface
{
    /// <summary>
    /// Opc客户端接口
    /// </summary>
    public interface IOpcClient : IDataChannel
    {
        /// <summary>
        /// 浏览地址
        /// </summary>
        /// <param name="address">地址</param>
        /// <returns>子地址</returns>
        IEnumerable<DataAddress> Explore(DataAddress address);

        /// <summary>
        /// 监视值变化
        /// </summary>
        /// <typeparam name="TValue">类型</typeparam>
        /// <param name="address">地址</param>
        /// <param name="callback">回调,第二个参数为unsubscribe方法</param>
        void Monitor<TValue>(DataAddress address, Action<TValue, Action> callback);
    }
}
